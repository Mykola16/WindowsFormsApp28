using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;


namespace WindowsFormsApp28
{
    public partial class Form1 : Form
    {
        private const string sqlTovarSelect = "select * from Tovar";
        private const string sqlProdazhiSelect = "select * from Prodazhi";
        private const string sqlGroupsSelect = "select * from Managers";
        private SqlConnection sqlConnection;

        public Form1(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;

            InitializeComponent();
            dgvT.MultiSelect = false;
            dgvT.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvT.AllowUserToAddRows = false;
            dgvT.AllowUserToDeleteRows = false;
            dgvT.ReadOnly = true;

            //sUpdateTechersView();
        }

        private void UpdateTechersView()
        {
            dgvT.DataSource = null;
            SqlDataAdapter adapter = new SqlDataAdapter(sqlTovarSelect, sqlConnection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dgvT.DataSource = table;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateTechersView();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;

            string sql = "WAITFOR DELAY '00:00:05'; INSERT INTO[Tovar]([Id],[Name],[Type],[Number]) VALUES(@Id,@Name,@Type,@Number)";
            SqlCommand command = new SqlCommand(sql, sqlConnection);
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Name", edFirstname.Text));
                command.Parameters.Add(new SqlParameter("@Type", edLastname.Text));
                command.Parameters.Add(new SqlParameter("@Number", textBox1.Text));


                SqlParameter dep = new SqlParameter("@Id", SqlDbType.Int);
                dep.Value = (int)edDepartment.Value;
                command.Parameters.Add(dep);
                // v1

                //await command.ExecuteNonQueryAsync();

                // v2 use callback

                var state = command.BeginExecuteNonQuery(ExecuteQueryCallback, command);

                //// v3
                //while (!state.IsCompleted)
                //{
                //    // что-то выполняем пока работает асинхронная команда
                //}
                // заверщаем асинхронный вызов
                //int result = command.EndExecuteNonQuery(state);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnAdd.Enabled = true;
                //UpdateTechersView();
            }
        }

        private void ExecuteQueryCallback(IAsyncResult result)
        {
            // получаем ссылку на объект, который запустил асинхронную опреацию
            SqlCommand command = result.AsyncState as SqlCommand;
            if (command == null)
                return;

            // завершаем асинхронную операцию
            int rowcount = command.EndExecuteNonQuery(result);

            // обновить DataGridView
            Action a = () =>
            {
                btnAdd.Enabled = true;
                UpdateTechersView();
            };
            if (InvokeRequired)
            {
                Invoke(a);
            }
            else
            {
                a();
            }
        }



        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            int idToDelete = Convert.ToInt32(textBox5.Text);

            
                string query = "DELETE FROM Tovar WHERE Id = @id";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {

                    command.Parameters.AddWithValue("@id", idToDelete);


                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Запись удалена успешно.");
                        UpdateTechersView();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить запись.");
                    }

                }
            
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdateGroup_Click(object sender, EventArgs e)
        {
            dgvGroups.DataSource = null;
            SqlDataAdapter adapter = new SqlDataAdapter(sqlGroupsSelect, sqlConnection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dgvGroups.DataSource = table;
        }

        private async void btnAddGroup_Click(object sender, EventArgs e)
        {
            string sql = "WAITFOR DELAY '00:00:05'; INSERT INTO[Managers]([Id], [Name], [Surname],[Last_Name]) VALUES(@Id,@Name,@Surname,@Last_Name)";

            btnAddManagers.Enabled = false;
            SqlCommand command = new SqlCommand(sql, sqlConnection);
            command.Parameters.Add(new SqlParameter("@Name", edGroupName.Text));
            command.Parameters.Add(new SqlParameter("@Surname", textBox2.Text));
            command.Parameters.Add(new SqlParameter("@Last_Name", textBox3.Text));


            SqlParameter dep1 = new SqlParameter("@Id", SqlDbType.Int);
            dep1.Value = (int)edFacultyId.Value;
            command.Parameters.Add(dep1);




            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnAddManagers.Enabled = true;
                btnUpdateGroup_Click(sender, e);
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            int idToDelete = Convert.ToInt32(textBox9.Text);

            
                string query = "DELETE FROM Managers WHERE Id = @id";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {

                    command.Parameters.AddWithValue("@id", idToDelete);


                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Запись удалена успешно.");
                        btnUpdateGroup_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить запись.");
                    }

                }
            
        }


        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            SqlDataAdapter adapter = new SqlDataAdapter(sqlProdazhiSelect, sqlConnection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string sql = "WAITFOR DELAY '00:00:05'; INSERT INTO[Prodazhi]([Id], [Id_Tovar], [Id_Managers],[Sobivartist]," +
                "[Name_Firma],[kolichestvo_prodanih_tovariv],[tsina_za_odinitsu],[data_prodazhu]) " +
                "VALUES(@Id,@Id_Tovar,@Id_Managers,@Sobivartist,@Name_Firma,@kolichestvo_prodanih_tovariv,@tsina_za_odinitsu,@data_prodazhu)";

            btnAddManagers.Enabled = false;
            SqlCommand command = new SqlCommand(sql, sqlConnection);
            command.Parameters.Add(new SqlParameter("@Sobivartist", textBox_Sob.Text));
            command.Parameters.Add(new SqlParameter("@Name_Firma", textBox_NameF.Text));
            command.Parameters.Add(new SqlParameter("@kolichestvo_prodanih_tovariv", UpDown_Kol.Text));
            command.Parameters.Add(new SqlParameter("@tsina_za_odinitsu", textBox_Tsina.Text));


            SqlParameter dep1 = new SqlParameter("@Id", SqlDbType.Int);
            dep1.Value = (int)Id_Prod.Value;
            command.Parameters.Add(dep1);

            SqlParameter dep2 = new SqlParameter("@Id_Tovar", SqlDbType.Int);
            dep2.Value = (int)Id_Tovar.Value;
            command.Parameters.Add(dep2);

            SqlParameter dep3 = new SqlParameter("@Id_Managers", SqlDbType.Int);
            dep3.Value = (int)Id_Man.Value;
            command.Parameters.Add(dep3);

            SqlParameter data_Prod = new SqlParameter("@data_prodazhu", SqlDbType.Date);
            data_Prod.Value = edBirthdate.Value;
            command.Parameters.Add(data_Prod);


            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnAddManagers.Enabled = true;
                button4_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int idToDelete = Convert.ToInt32(textBox4.Text);

          
                string query = "DELETE FROM Prodazhi WHERE Id = @id";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {

                    command.Parameters.AddWithValue("@id", idToDelete);


                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Запись удалена успешно.");
                        button4_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить запись.");
                    }

                }
            
        }
        //1)
        //Показати інформацію про менеджері з найбільшою
        //кількістю продажів за кількістю одиниць.

        //2)
        // Показати інформацію про менеджера з продажу з
        // найбільшою, загальною сумою прибутку.

        //3)
        //  Показати інформацію про менеджера з продажу з
        //найбільшою загальною сумою прибутку за вказаний
        //проміжок часу.

        //4)
        //  Показати інформацію про фірму-покупця, яка зробила
        //закупку на найбільшу суму.

        //5)
        //  Показати інформацію про тип канцтоварів з найбільшою
        //кількістю одиниць продажів.

        //6)
        //  Показати інформацію про тип найприбутковіших
        // канцтоварів.

        //7)
        //  Показати назву найпопулярніших канцтоварів.
        // Популярність вираховуємо за кількістю проданих одиниць.

        //8)
        //  Показати назву канцтоварів, які не продавалися у задану
        // кількість днів.

        private void button6_Click(object sender, EventArgs e)
        {
            

            //1
                string sqlQuery = "SELECT TOP 1 Prodazhi.Id_Managers, SUM(Prodazhi.kolichestvo_prodanih_tovariv) as Кількість_проданих_товарів_загалом " +
                    "FROM Prodazhi " +
                    "GROUP BY Prodazhi.Id_Managers " +
                    "ORDER BY SUM(Prodazhi.kolichestvo_prodanih_tovariv) / COUNT(DISTINCT Prodazhi.Id) DESC";

                using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView2.DataSource = dataTable;
                   
                }
             
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //2
            string sqlQuery1 = "SELECT TOP 1 Id_Managers, SUM(kolichestvo_prodanih_tovariv * tsina_za_odinitsu - sobivartist) as Suma_pribytku " +
                      "FROM Prodazhi " +
                      "GROUP BY Id_Managers " +
                      "ORDER BY Suma_pribytku DESC";

            using (SqlCommand command = new SqlCommand(sqlQuery1, sqlConnection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView2.DataSource = dataTable;

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            //3
            string sqlQuery2 = "SELECT TOP 1 " +
                     "Prodazhi.Id_Managers, " +
                     "SUM(Prodazhi.kolichestvo_prodanih_tovariv * Prodazhi.tsina_za_odinitsu - Prodazhi.Sobivartist) as Prybutok " +
                     "FROM " +
                     "Prodazhi " +
                     "JOIN Tovar ON Prodazhi.Id_Tovar = Tovar.Id " +
                     "WHERE " +
                     "Prodazhi.Data_prodazhu BETWEEN '2022-01-01' AND '2023-12-31' " +
                     "GROUP BY " +
                     "Prodazhi.Id_Managers " +
                     "ORDER BY " +
                     "Prybutok DESC";

            using (SqlCommand command = new SqlCommand(sqlQuery2, sqlConnection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable;
            }
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //5
            string query = "SELECT TOP 1  Tovar.Type, SUM(Prodazhi.kolichestvo_prodanih_tovariv) as Кількість_продаж " +
                  "FROM Prodazhi " +
                  "JOIN Tovar ON Prodazhi.Id_Tovar = Tovar.Id " +
                  "GROUP BY Tovar.Type " +
                  "ORDER BY Кількість_продаж DESC";

            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //6
            string sqlQuery = "SELECT TOP 1 " +
                         "Tovar.Type, " +
                         "SUM(Prodazhi.kolichestvo_prodanih_tovariv * Prodazhi.tsina_za_odinitsu - Prodazhi.Sobivartist) as Prybutok " +
                         "FROM Prodazhi " +
                         "JOIN Tovar ON Prodazhi.Id_Tovar = Tovar.Id " +
                         "GROUP BY Tovar.Type " +
                         "ORDER BY Prybutok DESC";
            using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //7
            string sqlQuery6 = "SELECT TOP 3 Tovar.Name, SUM(Prodazhi.kolichestvo_prodanih_tovariv) as KilkistProdanih " +
           "FROM Prodazhi " +
           "JOIN Tovar ON Prodazhi.Id_Tovar = Tovar.Id " +
           "GROUP BY Tovar.Name " +
           "ORDER BY KilkistProdanih DESC";

            using (SqlCommand command = new SqlCommand(sqlQuery6, sqlConnection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable;
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            //8
            string sqlQuery66 = "SELECT Tovar.Name " +
                          "FROM Tovar " +
                          "WHERE Tovar.Id NOT IN (" +
                          "  SELECT DISTINCT Prodazhi.Id_Tovar " +
                          "  FROM Prodazhi " +
                          "  WHERE Prodazhi.Data_prodazhu >= DATEADD(day, -10, GETDATE())" +
                          ")";

            using (SqlCommand command = new SqlCommand(sqlQuery66, sqlConnection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable;
            }
        }
    }
}

