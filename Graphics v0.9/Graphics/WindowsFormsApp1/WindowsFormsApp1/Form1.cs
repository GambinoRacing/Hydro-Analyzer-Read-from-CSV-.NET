using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SQLiteConnection Connection;

        public Form1()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            label3.Hide();
            label7.Hide();


            SQLiteConnection.CreateFile("hydrodb.sqlite");
            SQLiteConnection Connection = new SQLiteConnection("Data Source=hydrodb.sqlite;Version=3;");
            Connection.Open();

            string createTable = ("CREATE TABLE hyddnev (Station UNSIGNED INT(5) NOT NULL, Dat datetime NOT NULL, Stoej int(5) DEFAULT NULL, Vkol UNSIGNED FLOAT(7,3) DEFAULT NULL, PRIMARY KEY (Station, Dat))");
            SQLiteCommand createHydDnev = new SQLiteCommand(createTable, Connection);
            createHydDnev.ExecuteNonQuery();

            string createTable2 = ("CREATE TABLE hydmes (Station UNSIGNED INT(5) NOT NULL, Dat datetime NOT NULL, StoejMin smallint(5) DEFAULT NULL, VkolMin UNSIGNED FLOAT(7,3) DEFAULT NULL, StoejSre smallint(5) DEFAULT NULL, VkolSre UNSIGNED FLOAT(7,3) DEFAULT NULL, StoejMax smallint(5) DEFAULT NULL, VkolMax UNSIGNED FLOAT(7,3) DEFAULT NULL, PRIMARY KEY (Station, Dat))");
            SQLiteCommand createHydMes = new SQLiteCommand(createTable2, Connection);
            createHydMes.ExecuteNonQuery();

            string createTable3 = ("CREATE TABLE hydgod (Station UNSIGNED INT(5) NOT NULL, Dat datetime NOT NULL, God_MinQ UNSIGNED FLOAT(7,3) DEFAULT NULL, God_AverQ UNSIGNED FLOAT(7,3) DEFAULT NULL, God_MaxQ UNSIGNED FLOAT(7,3) DEFAULT NULL, PRIMARY KEY (Station, Dat))");
            SQLiteCommand createHydGod = new SQLiteCommand(createTable3, Connection);
            createHydGod.ExecuteNonQuery();

            this.Connection = Connection;

            /*
            try
            {
                string addDat = ("INSERT or IGNORE INTO hydmes (Station, Dat, VkolMin) values (2, '2017-10-10', 5); UPDATE hydmes SET VkolMin= 5 WHERE Station = 2 and Dat = '2017-10-10';");
                SQLiteCommand insertDat = new SQLiteCommand(addDat, Connection);
                int i = insertDat.ExecuteNonQuery();
               // MessageBox.Show(i.ToString());

                addDat = ("INSERT or IGNORE INTO hydmes (Station, Dat, VkolSre) values (2, '2017-10-10', 6); UPDATE hydmes SET VkolSre= 6 WHERE Station = 2 and Dat = '2017-10-10';");
                insertDat = new SQLiteCommand(addDat, Connection);
               i = insertDat.ExecuteNonQuery();
               // MessageBox.Show(i.ToString());

                addDat = ("INSERT or IGNORE INTO hydmes (Station, Dat, VkolMax) values (2, '2017-10-10', 7); UPDATE hydmes SET VkolMax = 7 WHERE Station = 2 and Dat = '2017-10-10'; ");
                insertDat = new SQLiteCommand(addDat, Connection);
                i = insertDat.ExecuteNonQuery();
              //  MessageBox.Show(i.ToString());

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            string test = "select Station,Dat, VkolMin, VkolSre, VkolMax from hydmes";
            SQLiteCommand testSelect = new SQLiteCommand(test, Connection);

            try
            {
                SQLiteDataReader r = testSelect.ExecuteReader();

                while (r.Read())
                {
                    MessageBox.Show(r["Dat"].ToString() + " - " + r["VkolMin"].ToString() + " - " + r["VkolSre"].ToString() + " - " + r["VkolMax"].ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            */
            
        }

        string pathFolder;
        string pathFolder2;

        string resultStation;
        string resultStation2;

        List<string> resultYears = new List<string>();
        List<string> resultYears2 = new List<string>();
        string addDat;

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    string sFileName = dialog.FileName;
                    pathFolder = sFileName;

                    label3.Text = pathFolder;
                    label3.Show();

                    string[] lines = System.IO.File.ReadAllLines(dialog.FileName);

                    int i = 0;

                    SQLiteCommand sqlComm;
                    sqlComm = new SQLiteCommand("begin", Connection);
                    sqlComm.ExecuteNonQuery();

                    foreach (var line in lines)
                    {

                        var splittedValues = line.Split(',');

                        var firstWord = splittedValues[0];
                        var firstYear = splittedValues[1];

                        if (!resultYears.Contains(firstYear))
                        {
                            resultYears.Add(firstYear);
                        }

                        if (i == 0)
                        {
                            resultStation = firstWord;
                        }
                        else
                        {
                            if (resultStation != firstWord)
                            {
                                MessageBox.Show("Файла с дневни данни трябва да съдържа само една станция!");
                                return;
                            }
                        }
                        i++;

                        string addDat = "";
                        if (splittedValues[3] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-01-" + (int.Parse(splittedValues[2])<10?"0":"") + splittedValues[2] + "', " + splittedValues[3] + ");";
                        }
                        if (splittedValues[4] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-02-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[4] + ");";
                        }
                        if (splittedValues[5] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-03-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[5] + ");";
                        }
                        if (splittedValues[6] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-04-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[6] + ");";
                        }
                        if (splittedValues[7] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-05-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[7] + ");";
                        }
                        if (splittedValues[8] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-06-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[8] + ");";
                        }
                        if (splittedValues[9] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-07-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[9] + ");";
                        }
                        if (splittedValues[10] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-08-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[10] + ");";
                        }
                        if (splittedValues[11] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-09-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[11] + ");";
                        }
                        if (splittedValues[12] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-10-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[12] + ");";
                        }
                        if (splittedValues[13] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-11-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[13] + ");";
                        }
                        if (splittedValues[14] != "")
                        {
                            addDat += "INSERT INTO hyddnev (Station, Dat, Vkol) values (" + resultStation + ", '" + splittedValues[1] + "-12-" + (int.Parse(splittedValues[2]) < 10 ? "0" : "") + splittedValues[2] + "', " + splittedValues[14] + ");";
                        }

                        try
                        {
                            if(addDat != "")
                            {
                                SQLiteCommand insertDat = new SQLiteCommand(addDat, Connection);
                                insertDat.ExecuteNonQuery();
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        this.addDat = addDat;
                    }

                    sqlComm = new SQLiteCommand("end", Connection);
                    sqlComm.ExecuteNonQuery();

                    //Проверявам колко реда са импортирани в базата
                    /*
                    string test = "select count(*) AS temp from hyddnev";
                    SQLiteCommand testSelect = new SQLiteCommand(test, Connection);

                    try
                    {
                        SQLiteDataReader r = testSelect.ExecuteReader();

                        while (r.Read())
                        {
                            MessageBox.Show(r["temp"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    */

                    resultYears.Sort();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    string sFileName = dialog.FileName;
                    pathFolder2 = sFileName;
        
                    label7.Text = pathFolder2;
                    label7.Show();

                    string[] lines = System.IO.File.ReadAllLines(dialog.FileName, Encoding.GetEncoding("windows-1251"));

                    int i = 0;

                    SQLiteCommand sqlComm;
                    sqlComm = new SQLiteCommand("begin", Connection);
                    sqlComm.ExecuteNonQuery();

                    foreach (var line in lines)
                    {
                        var splittedValues = line.Split(',');

                        var firstWord = splittedValues[0];
                        var firstYear2 = splittedValues[1];

                        if (!resultYears2.Contains(firstYear2))
                        {
                            resultYears2.Add(firstYear2);
                        }

                        if (i == 0)
                        {
                            resultStation2 = firstWord;
                        }
                        else
                        {
                            if (resultStation2 != firstWord)
                            {
                                MessageBox.Show("Файла с месечни данни трябва съдържа само една станция!");
                                return;
                            }
                        }

                        i++;

                        string addDat = "";
                        if (splittedValues[3] != "")
                        {
                            string Date = splittedValues[1] + "-01-01";
                            string Vkol = splittedValues[3];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }    
                        }
                        if (splittedValues[4] != "")
                        {
                            string Date = splittedValues[1] + "-02-01";
                            string Vkol = splittedValues[4];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }
                        if (splittedValues[5] != "")
                        {
                            string Date = splittedValues[1] + "-03-01";
                            string Vkol = splittedValues[5];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }
                        if (splittedValues[6] != "")
                        {
                            string Date = splittedValues[1] + "-04-01";
                            string Vkol = splittedValues[6];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }
                        if (splittedValues[7] != "")
                        {
                            string Date = splittedValues[1] + "-05-01";
                            string Vkol = splittedValues[7];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }
                        if (splittedValues[8] != "")
                        {
                            string Date = splittedValues[1] + "-06-01";
                            string Vkol = splittedValues[8];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }
                        if (splittedValues[9] != "")
                        {
                            string Date = splittedValues[1] + "-07-01";
                            string Vkol = splittedValues[9];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }
                        if (splittedValues[10] != "")
                        {
                            string Date = splittedValues[1] + "-08-01";
                            string Vkol = splittedValues[10];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }
                        if (splittedValues[11] != "")
                        {
                            string Date = splittedValues[1] + "-09-01";
                            string Vkol = splittedValues[11];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }
                        if (splittedValues[12] != "")
                        {
                            string Date = splittedValues[1] + "-10-01";
                            string Vkol = splittedValues[12];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }
                        if (splittedValues[13] != "")
                        {
                            string Date = splittedValues[1] + "-11-01";
                            string Vkol = splittedValues[13];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }
                        if (splittedValues[14] != "")
                        {
                            string Date = splittedValues[1] + "-12-01";
                            string Vkol = splittedValues[14];
                            if (splittedValues[2] == "НМ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMin) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMin= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "СР")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolSre) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolSre= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                            if (splittedValues[2] == "НГ")
                            {
                                addDat += "INSERT OR IGNORE INTO hydmes (Station, Dat, VkolMax) values (" + resultStation2 + ", '" + Date + "', " + Vkol + "); UPDATE hydmes SET VkolMax= " + Vkol + " WHERE Station = " + resultStation2 + " and Dat = '" + Date + "';";
                            }
                        }

                        string addGod = "";

                        if (splittedValues[2] == "")
                        {
                            //insert в hydgod.. 4ти 6ти и 8ми елемент. 

                            string Date = splittedValues[1] + "-12-31";
                            string Min = splittedValues[6];
                            string Avg = splittedValues[8];
                            string Max = splittedValues[10];

                            addGod += "INSERT INTO hydgod (Station, Dat, God_MinQ, God_AverQ, God_MaxQ) values (" + resultStation2 + ", '" + Date + "', " + Min + ", " + Avg + ", " + Max + ");";

                            try
                            {
                                if (addGod != "")
                                {
                                    SQLiteCommand insertHydGod = new SQLiteCommand(addGod, Connection);
                                    insertHydGod.ExecuteNonQuery();
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                            //MessageBox.Show(splittedValues[6].ToString());

                            /*
                            string test = "select count(*) AS temp from hydgod";
                            SQLiteCommand testSelect = new SQLiteCommand(test, Connection);

                            try
                            {
                                SQLiteDataReader r = testSelect.ExecuteReader();

                                while (r.Read())
                                {
                                    MessageBox.Show(r["temp"].ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            */
                        }
                       
                        try
                        {
                            if (addDat != "")
                            {
                                SQLiteCommand insertDat = new SQLiteCommand(addDat, Connection);
                                insertDat.ExecuteNonQuery();
                                
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    sqlComm = new SQLiteCommand("end", Connection);
                    sqlComm.ExecuteNonQuery();

                    /*
                    string test = "select count(*) AS temp from hydmes";
                    SQLiteCommand testSelect = new SQLiteCommand(test, Connection);

                    try
                    {
                        SQLiteDataReader r = testSelect.ExecuteReader();

                        while (r.Read())
                        {
                            MessageBox.Show(r["temp"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    */

                    resultYears2.Sort();
                }
            }
        }

        public void label3_Click(object sender, EventArgs e)
        {

        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (resultStation != resultStation2)
            {
                MessageBox.Show("Номера на станцията в единия файл не отговаря на номера на станцията в другият файл!" + Environment.NewLine + Environment.NewLine +
                    "ЗАБЕЛЕЖКА!" + Environment.NewLine + Environment.NewLine + "В двата файла, номера на станцията трябва да бъде един и същ!");
            }

            comboBox1.Items.Add(resultStation);

            if (string.Join(", ", resultYears) == string.Join(", ", resultYears2))
            //if (resultYears.Equals(resultYears2))
            {
                for (int i = 0; i < this.resultYears.Count; i++)
                {
                    comboBox2.Items.Add(resultYears[i]);
                }
            }
            else
            {
                MessageBox.Show("Годините от двата файла не съвпадат.");
            }

        }
  

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(comboBox1.Text) && String.IsNullOrEmpty(comboBox2.Text))
            {
                MessageBox.Show("Моля изберете година");
            }
            else
            {
                DataGridViewRow row = this.dataGridView1.RowTemplate;
                row.DefaultCellStyle.BackColor = Color.Bisque;
                row.Height = 25;

                string yearDate = comboBox2.SelectedItem.ToString();

                CalculateData(yearDate);
            }
        }
        
        /*
        private void datagridview_CellValidating(object sender, EventArgs e)
        {
            try
            {
                int bbor = dataGridView1.CurrentCell.ColumnIndex;
                int ebor = dataGridView1.CurrentCell.RowIndex;
                if (ebor < bbor)
                {
                    dataGridView1.CurrentCell.Style.BackColor = Color.Red;
                }
            }
            catch (Exception exception)
            {
            }
        }
        */

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void CalculateData(string yearDate)
        {
            string command = "select vkol from hyddnev where station='"
                + comboBox1.SelectedItem.ToString() + "' and strftime('%Y',Dat) = '"
                + yearDate + "' order by Dat";

            var StartDate = comboBox2.Text;
            var EndDate = comboBox2.Text;

            SQLiteCommand sqlComm;
            sqlComm = new SQLiteCommand("begin", Connection);
            sqlComm.ExecuteNonQuery();

            SQLiteDataAdapter insertDat = new SQLiteDataAdapter(command, Connection);
            DataTable dt = new DataTable();
            insertDat.Fill(dt);


            string s2 = "";
            int i = 0;
            chart1.Series["Avg"].Points.Clear();

            int startYear2 = 0;
            int.TryParse(StartDate, out startYear2);
            DateTime dtStart2 = new DateTime(startYear2, 01, 01);
            chart1.Series["Avg"].XValueType = ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            chart1.ChartAreas[0].AxisX.Interval = 7;
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chart1.ChartAreas[0].AxisX.IntervalOffset = 1;

            chart1.Series["Avg"].MarkerSize = 10;
            chart1.Series["Avg"].Color = Color.Blue;
            chart1.Series["Avg"].BorderWidth = 2;

            foreach (DataRow row in dt.Rows)
            {
                chart1.Series["Avg"].Points.AddXY(dtStart2.AddDays(i), row.ItemArray[0]);
                i++;

                string rowz = string.Format("Avg Q - {0}" + Environment.NewLine, row.ItemArray[0]);
                s2 += "" + rowz;
            }

            // Axis ax = chart1.ChartAreas[0].AxisX;
            // ax.ScaleView.Size = 30;
            // ax.ScaleView.Position = 30;
            chart1.Show();


            sqlComm = new SQLiteCommand("end", Connection);
            sqlComm.ExecuteNonQuery();


            sqlComm = new SQLiteCommand("begin", Connection);
            sqlComm.ExecuteNonQuery();

            string command2 = "select VkolMin,VkolSre,VkolMax,strftime('%Y',Dat) from hydmes where station='"
                + comboBox1.SelectedItem.ToString() + "' and strftime('%Y',Dat) = '"
                + yearDate + "' order by Dat";

            SQLiteDataAdapter insertDat2 = new SQLiteDataAdapter(command2, Connection);
            DataTable dt2 = new DataTable();
            insertDat2.Fill(dt2);

            string s1 = "";
            chart2.Series["Min"].Points.Clear();
            chart2.Series["Avg"].Points.Clear();
            chart2.Series["Max"].Points.Clear();
            chart2.ChartAreas[0].AxisX.Minimum = double.NaN;
            chart2.ChartAreas[0].AxisX.Maximum = double.NaN;
            chart2.ChartAreas[0].RecalculateAxesScale();

            chart2.Series["Min"].BorderWidth = 3;
            chart2.Series["Avg"].BorderWidth = 3;
            chart2.Series["Max"].BorderWidth = 3;

            i = 0;
            int startYear = 0;
            int.TryParse(StartDate, out startYear);
            DateTime dtStart = new DateTime(startYear, 01, 01);
            chart2.Series["Min"].XValueType = ChartValueType.DateTime;
            chart2.Series["Avg"].XValueType = ChartValueType.DateTime;
            chart2.Series["Max"].XValueType = ChartValueType.DateTime;
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            chart2.ChartAreas[0].AxisX.Interval = 1;
            chart2.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            chart2.ChartAreas[0].AxisX.IntervalOffset = 0;

            foreach (DataRow row in dt2.Rows)
            {
                chart2.Series["Avg"].Points.AddXY(dtStart.AddMonths(i), row.ItemArray[1]);
                chart2.Series["Max"].Points.AddXY(dtStart.AddMonths(i), row.ItemArray[2]);
                chart2.Series["Min"].Points.AddXY(dtStart.AddMonths(i), row.ItemArray[0]);

                i++;

                string rowz = string.Format("Min Q - {0}" + Environment.NewLine + "Avg Q - {1}"
                   + Environment.NewLine + "Max Q - {2}" + Environment.NewLine + "Year - {3}" + Environment.NewLine
                   + Environment.NewLine,
                   row.ItemArray[0], row.ItemArray[1], row.ItemArray[2],
                   row.ItemArray[3]);
                s1 += "" + rowz;
            }

            sqlComm = new SQLiteCommand("end", Connection);
            sqlComm.ExecuteNonQuery();

            int year = int.Parse(yearDate);

            string command3 = "";


            command3 += "SELECT DISTINCT 'Въведени' AS 'Въведени/Изчислени', Station AS '№ на станция', strftime('%Y',Dat) AS 'Година', 'НМ' AS 'НМ/СР/НГ', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01') AS 'Януари', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02') AS 'Февруари', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03') AS 'Март', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04') AS 'Април', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05') AS 'Май', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06') AS 'Юни', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07') AS 'Юли', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08') AS 'Август', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09') AS 'Септември', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat)= '10') AS 'Октомври', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat)= '11') AS 'Ноември', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat)= '12') AS 'Декември'"
                + " FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year
                + "' UNION"
                + " SELECT 'Изчислени', Station, strftime('%Y',Dat), 'НМ', (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '10' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '11' LIMIT 1), (SELECT ROUND(min(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '12' LIMIT 1)"
                + " FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year
                + "' UNION"
                + " SELECT 'Въведени', Station, strftime('%Y',Dat), 'СР', (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '10'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '11'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '12')"
                + " FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year
                + "' UNION"
                + " SELECT 'Изчислени', Station, strftime('%Y',Dat), 'СР', (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '10' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '11' LIMIT 1), (SELECT ROUND(avg(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '12' LIMIT 1)"
                + " FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year
                + "' UNION"
                + " SELECT 'Въведени', Station, strftime('%Y',Dat), 'НГ', (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '10'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '11'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '12')"
                + " FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year
                + "' UNION"
                + " SELECT 'Изчислени', Station, strftime('%Y',Dat), 'НГ', (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '10' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '11' LIMIT 1), (SELECT ROUND(max(vkol), 3) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '12' LIMIT 1)"
                + " FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "'";

            command3 += " group by strftime('%Y',Dat)";

            /*
            command3 += "SELECT 'Въведени' AS 'Въведени/Изчислени', Station AS '№ на станция', Dat AS 'Година', 'НМ' AS 'НМ/СР/НГ', strftime('%Y',Dat) AS year, (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01') AS 'Януари', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02') AS 'Февруари', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03') AS 'Март', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04') AS 'Април', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05') AS 'Май', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06') AS 'Юни', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07') AS 'Юли', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08') AS 'Август', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09') AS 'Септември', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat)= '10') AS 'Октомври', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat)= '11') AS 'Ноември', (SELECT ROUND(vkolmin, 3) FROM hydmes WHERE Station= " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat)= '12') AS 'Декември'"
                + " FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year
                + "' UNION"
                + " SELECT 'Изчислени', Station, Dat, 'НМ', strftime('%Y',Dat) AS year, (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '10' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '11' LIMIT 1), (SELECT ROUND(min(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '12' LIMIT 1)"
                + " FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year
                + "' UNION"
                + " SELECT 'Въведени', Station, Dat, 'СР', strftime('%Y',Dat) AS year, (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '10'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '11'), (SELECT ROUND(vkolsre, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '12')"
                + " FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year
                + "' UNION"
                + " SELECT 'Изчислени', Station, Dat, 'СР', strftime('%Y',Dat) AS year, (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '10' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '11' LIMIT 1), (SELECT avg(vkol) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '12' LIMIT 1)"
                + " FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year
                + "' UNION"
                + " SELECT 'Въведени', Station, Dat, 'НГ', strftime('%Y',Dat) AS year, (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '10'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '11'), (SELECT ROUND(vkolmax, 3) FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '12')"
                + " FROM hydmes WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year
                + "' UNION"
                + " SELECT 'Изчислени', Station, Dat, 'НГ', strftime('%Y',Dat) AS year, (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '01' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '02' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '03' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '04' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '05' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '06' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '07' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '08' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '09' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '10' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '11' LIMIT 1), (SELECT ROUND(max(vkol, 3)) FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "' and strftime('%m',Dat) = '12' LIMIT 1)"
                + " FROM hyddnev WHERE Station = " + comboBox1.SelectedItem.ToString() + " and strftime('%Y',Dat) = '" + year + "'";

                command3 += " group by year";
                */

            string command4 = "SELECT 'Въведени' AS 'Въведени/Изчислени', Station AS '№ на станция', strftime('%Y',Dat) AS 'Година', God_MinQ 'Мин. Q', God_AverQ 'Ср. Q', God_MaxQ 'Макс. Q' from hydgod where Station = '" + comboBox1.SelectedItem.ToString() + "' and strftime('%Y',Dat) = '" + year + "'"
            + " UNION"
            + " SELECT 'Изчислени', Station, strftime('%Y',Dat), ROUND(min(vkol), 3), ROUND(avg(vkol), 3), ROUND(max(vkol), 3) from hyddnev where Station = '" + comboBox1.SelectedItem.ToString() + "' and strftime('%Y',Dat) = '" + year + "'";

            SQLiteDataAdapter insertDat3 = new SQLiteDataAdapter(command3, Connection);
            using (DataTable dt3 = new DataTable())
            {
                insertDat3.Fill(dt3);
                dataGridView1.DataSource = dt3.DefaultView;
            }

            SQLiteDataAdapter insertGod2 = new SQLiteDataAdapter(command4, Connection);
            using (DataTable dt4 = new DataTable())
            {
                insertGod2.Fill(dt4);
                dataGridView2.DataSource = dt4.DefaultView;
            }

            //DataTable src1 = (DataTable)dataGridView1.DataSource;
            //DataTable src2 = (DataTable)dataGridView1.DataSource;

            //MessageBox.Show(src1 == null?"1":"0");
            string errorInfo = "";
            string[] months = { "Януари", "Февруари", "Март", "Април", "Май", "Юни", "Юли", "Август", "Септември", "Октомври", "Ноември", "Декември" };
            for (int k = 0; k < dataGridView1.Rows.Count - 3; k++)
            {
                var row1 = dataGridView1.Rows[k];
                var row2 = dataGridView1.Rows[k + 3];

                for (int j = 4; j < row1.Cells.Count; j++)
                {
                    if (k == 0)
                    {
                        if (float.Parse(row1.Cells[j].Value.ToString()) < float.Parse(row2.Cells[j].Value.ToString()))
                        {
                            dataGridView1.Rows[k].Cells[j].Style.BackColor = Color.Red;
                            dataGridView1.Rows[k + 3].Cells[j].Style.BackColor = Color.Red;
                            errorInfo += "Има въведена максимална стойност, която е по-малка от изчислената за месец " + months[j - 4] + ".\n";

                        }
                    }
                    if (k == 1)
                    {
                        if (float.Parse(row1.Cells[j].Value.ToString()) > float.Parse(row2.Cells[j].Value.ToString()))
                        {
                            dataGridView1.Rows[k].Cells[j].Style.BackColor = Color.Red;
                            dataGridView1.Rows[k + 3].Cells[j].Style.BackColor = Color.Red;
                            errorInfo += "Има въведена минимална стойност, която е по-голяма от изчислената за месец " + months[j - 4] + ".\n";
                        }
                    }
                    if (k == 2)
                    {
                        float x = float.Parse(row1.Cells[j].Value.ToString());
                        float y = float.Parse(row2.Cells[j].Value.ToString());
                        if (y > x * 1.05 || y < x * 0.95 || x < y * 0.95 || x > y * 1.05)
                        {
                            dataGridView1.Rows[k].Cells[j].Style.BackColor = Color.Red;
                            dataGridView1.Rows[k + 3].Cells[j].Style.BackColor = Color.Red;
                            errorInfo += "Има разлика с повече от 5 процента между въведената и изчислената средни стойности за месец " + months[j - 4] + ".\n";
                        }
                    }
                }
            }

            if (errorInfo != "")
            {
                MessageBox.Show(errorInfo);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
                    
            string folder = @"D:\Comments\";
            folder = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + "\\Comments";
            string path = Environment.ExpandEnvironmentVariables(folder + "\\" + comboBox1.SelectedItem.ToString() + ".txt");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            else
            {
                using (StreamWriter w = File.AppendText(path))
                {
                    w.WriteLine("№ на станция: " + comboBox1.SelectedItem.ToString() + "," + " Дата/час: " + DateTime.Now + "," + " Коментар: " + textBox1.Text + Environment.NewLine);

                    MessageBox.Show("Вашият коментар е записан в директория: " + path);
                }
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            string yearDate = comboBox2.SelectedItem.ToString();

            int nextYear = int.Parse(yearDate) + 1;

            comboBox2.SelectedItem = nextYear.ToString();
            comboBox2.Text = nextYear.ToString();

            CalculateData(nextYear.ToString());
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            string yearDate = comboBox2.SelectedItem.ToString();

            int previousYear = int.Parse(yearDate) - 1;

            comboBox2.SelectedItem = previousYear.ToString();
            comboBox2.Text = previousYear.ToString();

            CalculateData(previousYear.ToString());
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }
    }
}