﻿using MySql.Data.MySqlClient;
using Mysqlx;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bancoDeDados
{
    public partial class DatabaseForm : Form
    {
        private MySqlConnection _connection;

        private string host = "localhost";
        private string user = "root";
        private string password = "";
        private string database = "crud";
        private string table = "products";

        private string datasource;


        public DatabaseForm()
        {
            datasource = $"datasource={host};username={user};password={password};database={database}";

            InitializeComponent();
            InitializeUI();
            InitializeDatabaseConn();
        }

        private void InitializeDatabaseConn()
        {
            try
            {
                _connection = new MySqlConnection(datasource);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"\n\n--- Error initializing database connection ---\n\n--> {ex}");
                throw;
            }
        }

        private void InitializeUI()
        {
            listView1.Clear();

            listView1.View = View.Details;
            listView1.LabelEdit = true;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Add("ID", 115, HorizontalAlignment.Left);
            listView1.Columns.Add("Nome", 115, HorizontalAlignment.Left);
            listView1.Columns.Add("Preço", 115, HorizontalAlignment.Left);

            this.Controls.OfType<TextBox>().ToList().ForEach(t => t.Text = string.Empty);

        }

        // -- Select All
        private void selectAllBtn_Click(object sender, EventArgs e)
        {

            try
            {
                string sql = $"select * from {table};";
                _connection.Open();


                MySqlCommand exec = new MySqlCommand(sql, _connection);
                MySqlDataReader reader = exec.ExecuteReader();
                
                listView1.Items.Clear();

                while (reader.Read())
                {
                    string[] row = new string[reader.FieldCount];

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString();
                    }

                    var listViewLine = new ListViewItem(row);
                    listView1.Items.Add(listViewLine);
                }               


            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\n--- Error initializing database connection ---\n\n--> {ex}");
                throw;
            }
            finally { _connection.Close(); }


        }

        // -- Create
        private void insertBtn_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(nameInput.Text))
                {
                    MessageBox.Show("Nome incorreto! Insira novamente.");
                    return;
                }

                if (string.IsNullOrEmpty(priceInput.Text) || !double.TryParse(priceInput.Text, out double newPrice))
                {
                    MessageBox.Show("Preço incorreto! Insira novamente.");
                    return;
                }

                string sql = $"insert into {table} (name, price) values (@Name, @Price)";
                _connection.Open();

                MySqlCommand exec = new MySqlCommand(sql, _connection);
                exec.Parameters.AddWithValue("@Name", nameInput.Text);
                exec.Parameters.AddWithValue("@Price", priceInput.Text);

                int rowsAffected = exec.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Produto inserido com sucesso!");
                }
                else MessageBox.Show("Falha ao inserir produto. Tente novamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inserir o produto:\n\n{ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            nameInput.Text = string.Empty;
            priceInput.Text = string.Empty;
            selectAllBtn_Click(sender, e);
        }

        // -- Update Selected
        private void updateBtn_Click(object sender, EventArgs e)
        {

            try
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Selecione um produto para atualizar.");
                    return;
                }
                
                if (string.IsNullOrEmpty(nameInput.Text))
                {
                    MessageBox.Show("Nome incorreto! Insira novamente.");
                    return;
                }
                
                if (string.IsNullOrEmpty(priceInput.Text) || !double.TryParse(priceInput.Text, out double newPrice))
                {
                    MessageBox.Show("Preço incorreto! Insira novamente.");
                    return;
                }

                var selectedItem = listView1.SelectedItems[0];
                int selectedProductId = int.Parse(selectedItem.SubItems[0].Text);

                string sql = $"update {table} set name = @Name, price = @Price where id = @Id";
                _connection.Open();

                MySqlCommand exec = new MySqlCommand(sql, _connection);
                exec.Parameters.AddWithValue("@Name", nameInput.Text);
                exec.Parameters.AddWithValue("@Price", priceInput.Text);
                exec.Parameters.AddWithValue("@Id", selectedProductId);

                int rowsAffected = exec.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Produto atualizado com sucesso!");
                }
                else
                {
                    MessageBox.Show("Falha ao atualizar produto. Tente novamente.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar o produto:\n\n{ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            nameInput.Text = string.Empty;
            priceInput.Text = string.Empty;
            selectAllBtn_Click(sender, e);

        }

        // -- Delete All
        private void deleteAllBtn_Click(object sender, EventArgs e)
        {
            DialogResult confirmation = MessageBox.Show(
                "Tem certeza de que deseja deletar todos os registros? Esta ação não pode ser desfeita.",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmation == DialogResult.No)
            {
                return;
            }

            try
            {
                string sql = $"delete from {table}";
                _connection.Open();

                MySqlCommand exec = new MySqlCommand(sql, _connection);

                int rowsAffected = exec.ExecuteNonQuery();

                MessageBox.Show(
                    rowsAffected > 0
                        ? "Todos os registros foram deletados com sucesso!"
                        : "Nenhum registro foi encontrado para deletar.",
                    "Operação Concluída",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ocorreu um erro ao deletar os registros:\n\n{ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        // -- Test BD Connection
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                _connection.Open();
                MessageBox.Show(
                    "Conexão com o banco de dados realizada com sucesso!",
                    "Teste de Conexão",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao conectar ao banco de dados:\n\n{ex.Message}",
                    "Erro de Conexão",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        
    }
}
