
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HelloApp
{
    
    class Program1
    {
        static async Task Main(string[] args)
        {

            string connectionString = "Server=DESKTOP-5BD88QO\\SQLEXPRESS;Database=CompanyDB;Trusted_Connection=True;";
            Console.WriteLine($"Для того чтобы изменить зарплату - 1\nДля того чтобы обновить - 2 \nДля удаления пользователя - 3\nДля того чтобы добавить пользователя - 4");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                int n = Convert.ToInt32(Console.ReadLine());
               
                while (n != 0) {
                   
                    if (n == 1) { update(connection); }
                    if (n == 3) { delete(connection); }
                    if (n == 4) { add(connection); }
                    if (n == 2)
                    {
                        SqlCommand command = new SqlCommand("SELECT * FROM Employees", connection);
                        SqlDataReader reader = await command.ExecuteReaderAsync(); search(reader); }
                        n = Convert.ToInt32(Console.ReadLine());
                }
            }


            Console.Read();
        }
        static async void update( SqlConnection connection)
        {
            Console.WriteLine("Введите ID человека");
            int ID = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите SALARY человека");
            double salary = double.Parse(Console.ReadLine());
            Console.WriteLine(salary);
            SqlCommand command = new SqlCommand($"UPDATE Employees SET Salary={salary.ToString().Replace(',', '.')} WHERE EmployeeID={ID}", connection);
            int number = await command.ExecuteNonQueryAsync();
            Console.WriteLine($"Обновлено объектов: {number}");
        }

        static async void search(SqlDataReader reader)
        {

            if (reader.HasRows) // если есть данные
            {

                string column1 = reader.GetName(0);
                string column2 = reader.GetName(1);
                string column3 = reader.GetName(2);
                string column4 = reader.GetName(3);
                string column5 = reader.GetName(4);


                Console.WriteLine($"{column1}\t{column2}\t{column3}\t{column4}\t{column5}");

                while (await reader.ReadAsync()) // построчно считываем данные
                {
                    object EmployeeID = reader.GetValue(0);
                    object Firstname = reader.GetValue(1);
                    object Lastname = reader.GetValue(2);
                    object Position = reader.GetValue(3);
                    object Salary = reader.GetValue(4);

                    Console.WriteLine($"{EmployeeID} \t        {Firstname} \t        {Lastname} \t        {Position}\t        {Salary}");
                }

            }
            await reader.CloseAsync();
        }
        static async void delete(SqlConnection connection)
        {
            int ID = Convert.ToInt32(Console.ReadLine());
            SqlCommand command = new SqlCommand($"DELETE  FROM Employees WHERE EmployeeID = {ID}", connection);
            int number = await command.ExecuteNonQueryAsync();
            Console.WriteLine($"Удалено объектов: {number}");
        }

        static async void add(SqlConnection connection)
        {
            Console.WriteLine("Введите имя");
            string Firstname = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Введите Фамилию");
            string Lastname = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Введите Должность");
            string Position = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Введите заработную плату");
            string Salary = Convert.ToString(Console.ReadLine());
            SqlCommand command = new SqlCommand($"INSERT INTO Employees (Firstname, Lastname, Position, Salary) VALUES ('{Firstname}','{Lastname}','{Position}',{Salary})", connection);
            int number = await command.ExecuteNonQueryAsync();
            Console.WriteLine($"Добавлено объектов: {number}");
        }
    }
}
