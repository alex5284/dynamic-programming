using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int n, m;
        double[,] ReadMatrixFromGrid()
        {
            n = Convert.ToInt32(tbn.Text);
            m = Convert.ToInt32(tbm.Text);
            m++;
            double[,] matrix = new double[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = Convert.ToDouble(dgMatrix.Rows[i].Cells[j].Value);
                }
            }
            return matrix;
        }

        void Calc()
        {
            double[,] matrix = ReadMatrixFromGrid();
            List<double> invest = new List<double>();
            List<double> invest1 = new List<double>();
            invest1.Add(0);
            List<List<double>> P_n = new List<List<double>>();
            int c = 1;
            for (int i = 0; i < n; i++)
            {
                invest.Add(matrix[i, 0]);
                invest1.Add(matrix[i, 0]);
            }

            for (int i = 0; i < m; i++)
            {
                List<double> P_i = new List<double>();
                if (c < m)
                {
                    for (int j = 0; j < n; j++)
                    {
                        P_i.Add(matrix[j, c]);
                    }
                    P_n.Add(P_i);
                    c++;
                }
            }
            List<Tuple<List<double>, List<double>>> tuples = new List<Tuple<List<double>, List<double>>>();
            Tuple<List<double>, List<double>> t = new Tuple<List<double>, List<double>>(invest, P_n[0]);
            tuples.Add(t);
            c = 1;
            int c2 = 0;
            for (int k = 0; k < P_n.Count - 1; k++)
            {

                double[,] m = new double[invest.Count, invest.Count + 1];
                for (int i = 0; i < invest.Count; i++)
                {
                    for (int j = 0; j < invest1.Count; j++)
                    {
                        if (j == 0) m[i, j] = tuples[k].Item2[i];
                        else
                        {
                            double res = invest[i] - invest1[j];
                            if (res < 0) break;
                            else
                            {
                                if (res == 0) m[i, j] = P_n[c][i];
                                else
                                {
                                    m[i, j] = P_n[c][j - 1] + tuples[k].Item2[c2];
                                    c2--;
                                }
                            }
                        }
                    }
                    c2 = i;
                }
                c++;

                List<double> temp1 = new List<double>(), temp2 = new List<double>();
                for (int i = 0; i < invest.Count; i++)
                {
                    List<double> temp = new List<double>();
                    for (int j = 0; j < invest1.Count; j++)
                    {
                        temp.Add(m[i, j]);
                    }
                    temp1.Add(temp.Max());
                    temp2.Add(invest1[temp.IndexOf(temp.Max())]);
                }
                t = new Tuple<List<double>, List<double>>(temp2, temp1);
                tuples.Add(t);
            }

            double max_number = tuples[tuples.Count - 1].Item2.Max();
            int index = tuples[tuples.Count - 1].Item2.IndexOf(max_number);
            double max_index = tuples[tuples.Count - 1].Item1[index];
            double ostatok = invest.Max() - max_index;//60
            List<double> number = new List<double>();
            List<double> ind = new List<double>();
            ind.Add(max_index);
            c = 0;
            for (int i = tuples.Count - 2; i >= 0; i--)
            {
                int k = invest.IndexOf(ostatok);
                double n = tuples[i].Item2.GetRange(0, k + 1).Max();
                index = tuples[i].Item2.IndexOf(n);
                double inde = tuples[i].Item1[index];
                ostatok = ostatok - inde;
                number.Add(n); ind.Add(inde);
                if (ostatok == 0 && i != 0)
                {
                    c = i;
                    do
                    {
                        ind.Add(0);
                        c--;
                    } while (c != 0);
                    break;
                }
            }

            listBox1.Items.Clear();
            listBox1.Items.Add("Максимальний приріст випуску продукції на " + tuples.Count + " підприємствах при розподілі");
            listBox1.Items.Add("між ними " + invest.Max() + " тис.грн складає " + max_number + " тис.грн і буде отриманий якщо:");
            listBox1.Items.Add("");
            c = 1;
            for (int i = ind.Count - 1; i >= 0; i--)
            {
                if (ind[i] == 0) listBox1.Items.Add(c + " підприємству коштів не виділяти");
                else listBox1.Items.Add(c + " підприємству виділити " + ind[i]);
                c++;
            }
        }

        private void btnCalc_Click_1(object sender, EventArgs e)
        {
            Calc();
        }

        private void btnCreate2_Click_1(object sender, EventArgs e)
        {
            n = Convert.ToInt32(tbn.Text);
            m = Convert.ToInt32(tbm.Text);
            m++;
            dgMatrix.Rows.Clear();
            dgMatrix.Columns.Clear();
            int i = 0;
            for (i = 0; i < m; i++)
            {
                if (i == 0) dgMatrix.Columns.Add("X", "X");
                else dgMatrix.Columns.Add("g" + i.ToString() + "(x)", "g" + i.ToString() + "(x)");
            }

            dgMatrix.Rows.Add(n);
        }
    }
}
