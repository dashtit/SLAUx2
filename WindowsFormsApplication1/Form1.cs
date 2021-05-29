using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form // это код 1 формы
    {
        private int n; // это кол-во неизвестных
        private double eps; // а эьто точность
        public Form1()
        {
            InitializeComponent();  // контруктор
        }

        private void Button1_Click(object sender, EventArgs e) // при нажатии на кнопку создается матрица в datagridview такого размера какого было задано в кол-ве незиветсных
        {
            try
            {
                n = int.Parse(textBox1.Text); // берем значение ко-ва незвестных введенное в текстобокс
                eps = double.Parse(textBox2.Text); // тож самое для точности
                if (n > 0 && n < 40)
                {
                    Refresh(dataGridView1);
                    Refresh(dataGridView2); 
                    dataGridView2.Columns.Add("", "");
                    for (int i = 0; i < n; i++) //цикл добавляет по ячейке на каждую итераци. итерация = кол-во незвестных
                    {
                        dataGridView1.Columns.Add("", "");
                        dataGridView1.Rows.Add();
                        dataGridView2.Rows.Add();
                    }
                }
                else
                {
                    textBox1.Clear();
                    MessageBox.Show("Количество неизвестных должно быть больше 0!");
                }
            }
            catch (Exception ex)
            {
                textBox1.Clear();
                MessageBox.Show(ex.Message);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            double[,] A = new double[n, n];
            double[] B = new double[n];
            double[] X = new double[n];
            bool FLAG = true;

            for (int row = 0; row < dataGridView1.Rows.Count; row++)
                for (int column = 0; column < dataGridView1.Columns.Count; column++) // это циклы на проверку пустых значений или букв
                     
                    try   //проходиь по каждой ячейке в датагриде и смотрит число это или нет
                    {
                        if (Convert.ToString(dataGridView1.Rows[row].Cells[column].Value) != "") // заодно смотрим пустая ячейка или нет
                        {
                            A[row, column] = Convert.ToInt32(dataGridView1.Rows[row].Cells[column].Value);
                        }
                        else // если в ячейке на число вывкидываем мэсседжбкос
                        {
                            MessageBox.Show("Введите все значения", "Поля с числовыми значениями пусты", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Refresh(dataGridView1);
                            Refresh(dataGridView2);
                            FLAG = false;
                        }
                    } 
                    catch (Exception ex)
                    {
                        dataGridView1.Rows[row].Cells[column].Value = "";
                        MessageBox.Show(ex.Message);
                        FLAG = false;
                    }

            for (int row = 0; row < dataGridView2.Rows.Count; row++) //это та же самая проверка но для матрицы из datagridview2 там где свободные члены
                try
                {
                    if (Convert.ToString(dataGridView2.Rows[row].Cells[0].Value) != "")
                    {
                        B[row] = Convert.ToInt32(dataGridView2.Rows[row].Cells[0].Value);
                    }
                    else
                    {
                        MessageBox.Show("Введите все значения", "Поля с числовыми значениями пусты", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Refresh(dataGridView1);
                        Refresh(dataGridView2);
                        FLAG = false;
                       
                    }
                }
                catch (Exception ex)
                {
                    dataGridView2.Rows[row].Cells[0].Value = "";
                    MessageBox.Show(ex.Message);
                    FLAG = false; 
                }

            
            ////////////////////////////////////////////////////////////////////////////////////////////
            bool flag = false;
            int counter1 = 0;
            int counter2 = 0;
            for (int i = 0; i < n; i++)
            {
                if (A[n-1, n-1] == 0)
                counter1++;
            }
            if (counter1 == n)
            {
                flag = true;
            }
            bool flag2 = false;
            if (flag == true)
            {
                for (int i = 0; i < n; i++)
                {
                    if (B[n-1] == 0)
                    counter2++;
                }
            }
            if (counter2 == n)
            {
                flag2 = true;
            }

            if (flag == true && flag2 == false && FLAG == true)
            {
                MessageBox.Show("Данная матрица решений не имеет", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Refresh(dataGridView3);

                dataGridView3.Columns.Add("", "");

                int accuracy = 0;
                while (eps < 1)
                {
                    eps *= 10;
                    accuracy++;
                }

                for (int row = 0; row < X.Length; row++) // здесь мы выводим рещультаты в третий датагрид 
                {

                    dataGridView3.Rows.Add();
                    dataGridView3.Rows[row].Cells[0].Value = Math.Round(X[row], accuracy);

                }
            }
            else if (flag == true && flag2 == true && FLAG == true)
                {
                MessageBox.Show("Данная матрица имеет бесконечно много решений", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Refresh(dataGridView3);
            } 
            else
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                        if (i == j)
                        {
                            SimpleIter.Div(ref A, ref B[i], A[i, j], i); // если прошло проверку отправляем считаться в simpleiter
                            X[i] = B[i];
                        }
                }
                SimpleIter.Solve(A, B, ref X, eps); // считаем
                Refresh(dataGridView3);

                dataGridView3.Columns.Add("", "");

                int accuracy = 0;
                while (eps < 1)
                {
                    eps *= 10;
                    accuracy++;
                }

                for (int row = 0; row < X.Length; row++) // здесь мы выводим рещультаты в третий датагрид 
                {

                    dataGridView3.Rows.Add();
                    dataGridView3.Rows[row].Cells[0].Value = Math.Round(X[row], accuracy);

                }
            }
          
            /////////////////////////////////////////////////////////
        }

        private void Refresh(DataGridView dataGridView) // ф-ция которую будем вызывать при нажатии на копнку начать сначала
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();
        }
        private void Button4_Click(object sender, EventArgs e) // а вот и эта кнопка тут мы все чистим 
        {
            Refresh(dataGridView1);
            Refresh(dataGridView2);
            Refresh(dataGridView3);
            textBox1.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2(); //кнопка помощи открывает вторую форму
            form2.Show();
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
