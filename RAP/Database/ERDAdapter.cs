using RAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; //for generating a MessageBox upon encountering an error
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace RAP
{
    abstract class ERDAdapter
    {
        //If including error reporting within this class (as this sample does) then you'll need a way
        //to control whether the errors are actually shown or silently ignored, since once you have
        //connected the GUI to the Boss object then the GUI designer will execute its code, which
        //will try to connect to the database to load live data into the GUI at design time.
        private static bool reportingErrors = false;

        //These would not be hard coded in the source file normally, but read from the application's settings (and, ideally, with some amount of basic encryption applied)
        private const string db = "kit206";
        private const string user = "kit206";
        private const string pass = "kit206";
        private const string server = "alacritas.cis.utas.edu.au";

        private static MySqlConnection conn = null;

        //Part of step 2.3.3 in Week 8 tutorial. This method is a gift to you because .NET's approach to converting strings to enums is so horribly broken
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Creates and returns (but does not open) the connection to the database.
        /// </summary>
        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                //Note: This approach is not thread-safe
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }

        //For step 2.2 in Week 8 tutorial
        public static List<Researcher> LoadAll()
        {
            List<Researcher> staff = new List<Researcher>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select id, type, given_name, family_name, title, unit, campus, email, photo, degree, level, utas_start, current_start from researcher", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Researcher re = new Researcher();
                    //Note that in your assignment you will need to inspect the *type* of the
                    //employee/researcher before deciding which kind of concrete class to create.
                    Researcher res = new Researcher();

                    res.ID = rdr.GetInt32(0);
                    res.Type = rdr.GetString(1);
                    res.Name = rdr.GetString(2) + " " + rdr.GetString(3);
                    res.Title_rdr = rdr.GetString(4);
                    res.Unit = rdr.GetString(5);
                    res.Campus = rdr.GetString(6);
                    res.Email = rdr.GetString(7);
                    res.Photo = rdr.GetString(8);
                    if (!rdr.IsDBNull(rdr.GetOrdinal("degree")))
                    {
                        res.Degree = rdr.GetString(9);
                    }
                    //in the SQL database, the "level" are in enum format, to change it to string is needed

                    if (!rdr.IsDBNull(rdr.GetOrdinal("level")))
                    {
                        res.level = ParseEnum<emp_level>(rdr.GetString(10));
                    }
                    res.UTAS_start = rdr.GetDateTime(11);
                    res.Current_start = rdr.GetDateTime(12);
                    staff.Add(res);
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading staff", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return staff;
        }

        //Researcher list view - publication list
        public static List<Publication> LoadPublication(int id)
        {
            List<Publication> work = new List<Publication>();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from researcher_publication rp inner join publication p on rp.doi = p.doi where researcher_id = ?id", conn);

                cmd.Parameters.AddWithValue("id", id);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    work.Add(new Publication
                    {

                        Title = rdr.GetString(3),
                        Year = rdr.GetInt32(5),
                        Authors = rdr.GetString(4),
                        Cite_as = rdr.GetString(7),
                        Mode = ParseEnum<Mode>(rdr.GetString(6)),
                        Certified = rdr.GetDateTime(8),
                        DOI = rdr.GetString(2)
                    });
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading training sessions", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return work;
        }

        public static List<Position> LoadPostions(int ID)
        {
            List<Position> positions = new List<Position>();
            MySqlConnection conn = ERDAdapter.GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select level, start, end from position as p where p.id=?id", conn);

                cmd.Parameters.AddWithValue("id", ID);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())

                {
                    Position p = new Position();

                    int levelOrd = rdr.GetOrdinal("level");
                    int startOrd = rdr.GetOrdinal("start");
                    int endOrd = rdr.GetOrdinal("end");

                    if (!rdr.IsDBNull(levelOrd))
                    {
                        p.Level = ParseEnum<emp_level>(rdr.GetString("level"));
                    }

                    if (!rdr.IsDBNull(startOrd))
                    {
                        p.Start = rdr.GetDateTime("start");
                    }

                    if (!rdr.IsDBNull(endOrd))
                    {
                        p.End = rdr.GetDateTime("end");
                    }

                    positions.Add(p);
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading training sessions", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return positions;
        }

        //get full publication details from database, used in publication detail view
        public Publication FetchFullPublicationDetails(string selectedDOI)
        {
            Publication p = new Publication();

            MySqlConnection conn = GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select doi, authors, year, title, type, available, cite_as " +
                                                    "from publication as p where p.doi=?doi", conn);
                cmd.Parameters.AddWithValue("doi", selectedDOI);

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    p.DOI = rdr.GetString(0);
                    p.Authors = rdr.GetString(1);
                    p.Year = rdr.GetInt32(2);
                    p.Title = rdr.GetString(3);
                    p.Mode = ParseEnum<Mode>(rdr.GetString(2));
                    p.Certified = rdr.GetDateTime(5);
                    p.Cite_as = rdr.GetString(6);
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading basic researcher detail", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return p;
        }

        public static int fetchThreeYearCount(int researcherID)
        {
            List<Publication> publications = new List<Publication>();
            MySqlConnection conn = ERDAdapter.GetConnection();
            MySqlDataReader rdr = null;

            int currentYear = DateTime.Today.Year;
            int startYear = currentYear - 3;

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                MySqlCommand cmd = new MySqlCommand("select year, title, type, respub.doi " +
                                                    "from publication as p ,researcher_publication as respub " +
                                                    "where p.doi=respub.doi and respub.researcher_id=?id and p.year>?startYear and p.year<=?currentYear", conn);

                cmd.Parameters.AddWithValue("id", researcherID);
                cmd.Parameters.AddWithValue("startYear", startYear);
                cmd.Parameters.AddWithValue("currentYear", currentYear);

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Publication p = new Publication();

                    p.Year = rdr.GetInt32(0);
                    p.Title = rdr.GetString(1);
                    p.Mode = ParseEnum<Mode>(rdr.GetString(2));
                    p.DOI = rdr.GetString(3);

                    publications.Add(p);
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading training sessions", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return publications.Count;
        }

        //supervision calculation count
        public static List<Researcher> FetchSupervisions(int ID)
        {
            List<Researcher> supervisions = new List<Researcher>();
            MySqlConnection conn = ERDAdapter.GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select id, title, given_name, family_name from researcher as r where r.supervisor_id=?id", conn);

                cmd.Parameters.AddWithValue("id", ID);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Researcher r = new Researcher();

                    r.ID = rdr.GetInt32(0);
                    r.Title_rdr = rdr.GetString(1);
                    r.Name = rdr.GetString(2) + " " + rdr.GetString(3);
                    supervisions.Add(r);
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading training sessions", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return supervisions;
        }

        //get supervisor name
        public static string FetchSupervisorName(int ID)
        {
            string supervisorName = null;
            MySqlConnection conn = ERDAdapter.GetConnection();
            MySqlDataReader rdr = null;

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("select r2.given_name, r2.family_name " +
                                                    "from researcher r1, researcher r2 " +
                                                    "where r1.supervisor_id=r2.id and r1.id=?id", conn);

                cmd.Parameters.AddWithValue("id", ID);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    supervisorName = rdr.GetString(0) + " " + rdr.GetString(1);
                }
            }
            catch (MySqlException e)
            {
                ReportError("loading training sessions", e);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return supervisorName;

        }
        /// <summary>
        /// In a more complete application this error would be logged to a file
        /// and the error reported back to the original caller, who is closer
        /// to the GUI and hence better able to produce the error message box
        /// (which would not show the actual error details like this does).
        /// </summary>
        private static void ReportError(string msg, Exception e)
        {
            if (reportingErrors)
            {
                MessageBox.Show("An error occurred while " + msg + ". Try again later.\n\nError Details:\n" + e,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
