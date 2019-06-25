using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Client
{
    class ClientFunc
    {
        string db_name = "C:\\Users\\Admin\\source\\repos\\KursaChat\\KursaChat.db";
        SQLiteConnection m_dbConnection;

        public string SignIn_ban(string uspa)
        {
            m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");
            m_dbConnection.Open();
            string message, us, res = "-1";
            string clientCode = uspa.Substring(0, 2);
            message = uspa.Substring(2);
            int indexOfChar = message.IndexOf("¬");
            us = message.Substring(0, indexOfChar);

            string sqlBan = "SELECT COUNT(*) FROM BanUsers WHERE Username = '" + us + "'";
            SQLiteCommand commandB = new SQLiteCommand(sqlBan, m_dbConnection);
            object readerB = commandB.ExecuteScalar();
            int existB = int.Parse(readerB.ToString());
            if (existB == 1)
            {
                res = "sub";
            }
            return res;
        }
        public string SignIn_uNo(string uspa)
        {
            m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");
            m_dbConnection.Open();
            string message, us, pass, res = "-1";
            string clientCode = uspa.Substring(0, 2);
            message = uspa.Substring(2);
            int indexOfChar = message.IndexOf("¬");
            us = message.Substring(0, indexOfChar);

            string sqlCL = "SELECT COUNT(*) FROM Users WHERE Username = '" + us + "'";
            SQLiteCommand command = new SQLiteCommand(sqlCL, m_dbConnection);
            object reader = command.ExecuteScalar();
            int exist = int.Parse(reader.ToString());
            pass = message.Substring(indexOfChar + 1);

            if (exist == 0)
            {
                string add = "INSERT INTO Users(Username, Password) VALUES('" + us + "', '" + pass + "')";
                SQLiteCommand command1 = new SQLiteCommand(add, m_dbConnection);
                command1.ExecuteNonQuery();
                string sqlP = "SELECT COUNT(*) FROM Users WHERE Username = '" + us + "'";
                SQLiteCommand commandP = new SQLiteCommand(sqlCL, m_dbConnection);
                object readerP = command.ExecuteScalar();

                int existP = int.Parse(readerP.ToString());

                if (existP == 1)
                {
                    string sqlCP = "SELECT COUNT(*) FROM Users WHERE Username = '" + us + "' AND Password = '" + pass + "'";
                    SQLiteCommand command2 = new SQLiteCommand(sqlCP, m_dbConnection);
                    object reader1 = command2.ExecuteScalar();
                    exist = int.Parse(reader1.ToString());
                    if (exist == 0)
                    {
                        res = "swp";
                    }
                    else
                    {
                        res = "srp";
                    }
                }
            }
            return res;
        }
        public string SignIn_uYes(string uspa)
        {
            m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");
            m_dbConnection.Open();
            string message, us, pass, res = "-1";
            string clientCode = uspa.Substring(0, 2);
            message = uspa.Substring(2);
            int indexOfChar = message.IndexOf("¬");
            us = message.Substring(0, indexOfChar);

            string sqlCL = "SELECT COUNT(*) FROM Users WHERE Username = '" + us + "'";
            SQLiteCommand command = new SQLiteCommand(sqlCL, m_dbConnection);
            object reader = command.ExecuteScalar();
            int exist = int.Parse(reader.ToString());
            pass = message.Substring(indexOfChar + 1);

            if (exist == 1)
            {
                string sqlCP = "SELECT COUNT(*) FROM Users WHERE Username = '" + us + "' AND Password = '" + pass + "'";
                SQLiteCommand command2 = new SQLiteCommand(sqlCP, m_dbConnection);
                object reader1 = command2.ExecuteScalar();
                int exist1 = int.Parse(reader1.ToString());
                if (exist1 == 0)
                {
                    res = "swp";
                }
                else
                {
                    res = "srp";
                }
            }
            return res;
        }
        public string OneMes(string mes)
        {
            string path = "C:\\Users\\Admin\\source\\repos\\KursaChat\\TestLogList.txt";
            string appendText = "";
            for (int i = 0; i < 3; i++)
            {
                appendText += mes;
            }
            File.AppendAllText(path, appendText);
            string res = File.ReadAllText(path);
            return res;
        }
    }
}
