using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Spire.Pdf;
using Spire.Pdf.Actions;
using Spire.Pdf.Bookmarks;
using Spire.Pdf.General;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;

namespace 测试spirepdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            PdfDocument doc = new PdfDocument();


            for (int i = 0; i < 4; i++)
            {
                PdfImage im = PdfImage.FromFile("00"+i.ToString()+".jpg");
                float width = im.Width;
                float height = im.Height;
                PdfPageBase page = doc.Pages.Add(new SizeF(width, height), new PdfMargins(0, 0, 0, 0));
                page.Canvas.DrawImage(im, 0, 0, width, height);
            }

            PdfImage im2 = PdfImage.FromFile("021.jpg");
            float width2 = im2.Width;
            float height2 = im2.Height;
            PdfPageBase page2 = doc.Pages.Add(new SizeF(width2, height2), new PdfMargins(0, 0, 0, 0));
            page2.Canvas.DrawImage(im2, 0, 0, width2, height2);


            //Save pdf file.
            doc.SaveToFile("MyFirstPDF.pdf");
            doc.Close();
            MessageBox.Show("ok");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile("MyFirstPDF.pdf");

            doc.Bookmarks.Clear();
            doc.SaveToFile("MyFirstPDF.pdf");
            doc.Dispose();
            MessageBox.Show("ok");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile("MyFirstPDF.pdf");
            for (int i = 0; i < doc.Pages.Count; i++)
            {
                PdfBookmark bookmark = doc.Bookmarks.Add("the" + i.ToString() + "page");//添加书签标题
                bookmark.Destination= new PdfDestination(doc.Pages[i],new PointF(0,0));//设置目的地
               
                bookmark.DisplayStyle = PdfTextStyle.Bold;
                bookmark.Color = Color.Black;

                //设置子目录
                //for (int j = 0; j < 5; j++)
                //{
                //    PdfBookmark childbookmark = bookmark.Insert(j, "the" + j.ToString() + "page");
                //    childbookmark.Destination = new PdfDestination(doc.Pages[i],new PointF(0, 0));
                //    childbookmark.Destination.Location = new PointF(0, 0);
                //    childbookmark.DisplayStyle = PdfTextStyle.Bold;
                //    childbookmark.Color = Color.Blue;
                //}
            }
            doc.SaveToFile("MyFirstPDF.pdf");
            doc.Dispose();
            MessageBox.Show("ok");

        }

   

        private void button5_Click(object sender, EventArgs e)
        {
            // 读取pdf
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile("基础2000试题.pdf", FileFormat.PDF);

            // 读取书签txt
            string[] lines = System.IO.File.ReadAllLines(@"书签.txt");
            int bookmark第一级 = -1;
            int bookmark第二级 = -1;
            foreach (string line in lines)
            {
                string[] s = line.Split('\t');
                if (s.Length == 2)
                {
                    // 查找
                    PdfTextFindCollection findCollection1 = doc.Pages[int.Parse(s[1])-1].FindText(s[0]);
                    for (int i = 0; i < findCollection1.Finds.Length; i++)
                    {
                        Console.WriteLine(line);
                        // 添加书签
                        PdfBookmark bookmark = doc.Bookmarks.Add(s[0]);//添加书签标题
                        bookmark.Destination = new PdfDestination(doc.Pages[int.Parse(s[1])-1], new PointF(findCollection1.Finds[i].Position.X, findCollection1.Finds[i].Position.Y - 43));//设置目的地
                        //bookmark.DisplayStyle = PdfTextStyle.Bold;
                        //bookmark.Color = Color.Black;
                    }
                    bookmark第一级++;
                }
                if (s.Length == 3)
                {
                    // 查找
                    PdfTextFindCollection findCollection1 = doc.Pages[int.Parse(s[2]) - 1].FindText(s[1]);
                    for (int i = 0; i < findCollection1.Finds.Length; i++)
                    {
                        Console.WriteLine(line);
                        // 添加书签
                        PdfBookmark bookmark = doc.Bookmarks[bookmark第一级].Add(s[1]);//添加书签标题
                        bookmark.Destination = new PdfDestination(doc.Pages[int.Parse(s[2]) - 1], new PointF(findCollection1.Finds[i].Position.X, findCollection1.Finds[i].Position.Y - 43));//设置目的地
                        //bookmark.DisplayStyle = PdfTextStyle.Bold;
                        //bookmark.Color = Color.Black;
                    }
                    bookmark第二级++;
                }
                if (s.Length == 4)
                {
                    // 查找
                    PdfTextFindCollection findCollection1 = doc.Pages[int.Parse(s[3]) - 1].FindText(s[2]);
                    for (int i = 0; i < findCollection1.Finds.Length; i++)
                    {
                        Console.WriteLine(line);
                        // 添加书签
                        PdfBookmark bookmark = doc.Bookmarks[bookmark第一级][bookmark第二级].Add(s[2]);//添加书签标题
                        bookmark.Destination = new PdfDestination(doc.Pages[int.Parse(s[3]) - 1], new PointF(findCollection1.Finds[i].Position.X, findCollection1.Finds[i].Position.Y - 43));//设置目的地
                        //bookmark.DisplayStyle = PdfTextStyle.Bold;
                        //bookmark.Color = Color.Black;
                    }
                }
            }

            // 保存
            doc.SaveToFile("2.pdf");
            doc.Dispose();
            MessageBox.Show("ok");
            System.Diagnostics.Process.Start("2.pdf");
        }
    }
}
