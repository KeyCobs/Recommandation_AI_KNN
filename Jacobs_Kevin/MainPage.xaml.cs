using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Jacobs_Kevin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Input userInput = new Input();
        InputHandler inHandl = new InputHandler();
        List<Products> products = new List<Products>();
        List<Products> pwp = new List<Products>(); // Products with points (pwp)
        List<Products> knnl = new List<Products>(); // Products with points (pwp)
        
        public MainPage()
        {
            this.InitializeComponent();
            ReadData("Assets/data_set_1.csv");
            //UpdateScreen();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            userInput = inHandl.GetInput(inputText.Text);
            //remove punctions 

            //Give everything points
            AddRelevant();
            //K nearest
            KNN();
            Console.WriteLine(this);
            UpdateScreen();
        }
        private void ReadData(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);

            foreach (var elem in System.IO.File.ReadAllLines(fileName))
            {
                //Uniq_Id	Product_Name	Category	Selling_Price	Image	Reviews
                try
                {
                    string[] el = elem.Split(',');
                    Products data = new Products();
                    if (el[5].Length > 10)
                    {
                        continue;
                    }
                    data.m_ID = el[0];
                    data.m_ProductName = el[1];
                    data.m_Category = el[2];
                    data.m_Price = el[3];
                    data.m_ImageURL = el[4];
                    data.m_NumberReviews = el[5];
                    products.Add(data);
                }
                catch (Exception)
                {

                    
                }

            }
        }


        private void ProductNamePanel()
        {
            ListView nameList = new ListView();
            ListView priceList = new ListView();
            ListView reviewList = new ListView();
            ListView CategoryList = new ListView();


            nameList.Items.Add("ProductName: ");
            CategoryList.Items.Add("Category:");
            reviewList.Items.Add("Amount of reviews:");
            priceList.Items.Add("Price: ");
            //pwp

            //for (int i = 0; i < knnl.Count; i++)
            //{
            //    //Space are underscores
            //    knnl[i].m_ProductName = knnl[i].m_ProductName.Replace('_', ' ');
            //    knnl[i].m_Category = knnl[i].m_Category.Replace('_', ' ');
            //    knnl[i].m_NumberReviews = knnl[i].m_NumberReviews.Replace('_', ' ');
            //    knnl[i].m_Price = "$" + knnl[i].m_Price.Replace('_', ' ');

            //    //adding for the stackPanel
            //    nameList.Items.Add(knnl[i].m_ProductName);
            //    CategoryList.Items.Add(knnl[i].m_Category);
            //    reviewList.Items.Add(knnl[i].m_NumberReviews);
            //    priceList.Items.Add(knnl[i].m_Price);
            //}

            for (int i = 0; i < pwp.Count; i++)
            {
                //Space are underscores
                pwp[i].m_ProductName = pwp[i].m_ProductName.Replace('_', ' ');
                pwp[i].m_Category = pwp[i].m_Category.Replace('_', ' ');
                pwp[i].m_NumberReviews = pwp[i].m_NumberReviews.Replace('_', ' ');
                pwp[i].m_Price = "$" + pwp[i].m_Price.Replace('_', ' ');

                //adding for the stackPanel
                nameList.Items.Add(pwp[i].m_ProductName);
                CategoryList.Items.Add(pwp[i].m_Category);
                reviewList.Items.Add(pwp[i].m_NumberReviews);
                priceList.Items.Add(pwp[i].m_Price);
            }


            NamePanel.Children.Add(nameList);
            ReviewsPanel.Children.Add(reviewList);
            PricePanel.Children.Add(priceList);
            CategoryPanel.Children.Add(CategoryList);
        }
        private void KNNPanel()
        {
            ListView nameList = new ListView();
            ListView priceList = new ListView();
            ListView reviewList = new ListView();
            ListView CategoryList = new ListView();


            //pwp
            nameList.Items.Add("ProductName: ");
            CategoryList.Items.Add("Category:");
            reviewList.Items.Add("Amount of reviews:");
            priceList.Items.Add("Price: ");
            for (int i = 0; i < knnl.Count; i++)
            {
                //Space are underscores
                knnl[i].m_ProductName = knnl[i].m_ProductName.Replace('_', ' ');
                knnl[i].m_Category = knnl[i].m_Category.Replace('_', ' ');
                knnl[i].m_NumberReviews = knnl[i].m_NumberReviews.Replace('_', ' ');
                knnl[i].m_Price = "$" + knnl[i].m_Price.Replace('_', ' ');

                //adding for the stackPanel
                nameList.Items.Add(knnl[i].m_ProductName);
                CategoryList.Items.Add(knnl[i].m_Category);
                reviewList.Items.Add(knnl[i].m_NumberReviews);
                priceList.Items.Add(knnl[i].m_Price);
            }

            NamePanel.Children.Add(nameList);
            ReviewsPanel.Children.Add(reviewList);
            PricePanel.Children.Add(priceList);
            CategoryPanel.Children.Add(CategoryList);
        }
        private void ClearPanels()
        {
            while (NamePanel.Children.Count > 0)
            {
                NamePanel.Children.RemoveAt(NamePanel.Children.Count - 1);
                ReviewsPanel.Children.RemoveAt(ReviewsPanel.Children.Count - 1);
                PricePanel.Children.RemoveAt(PricePanel.Children.Count - 1);
                CategoryPanel.Children.RemoveAt(CategoryPanel.Children.Count - 1);
                ImagePanel.Children.RemoveAt(ImagePanel.Children.Count - 1);
            }
            ImagePanel.Children.Clear();
        }
        private void UpdateScreen()
        {
            pwp.OrderBy(x => x.m_Value);
            ClearPanels();
            ProductNamePanel();
            //KNNPanel();

            //image
            ListView productsList = new ListView();

            //products[i].m_ImageURL.Split('|')[0]
            for (int i = 0; i < pwp.Count; i++)
            {
                Image imgB = new Image();
                BitmapImage btpImg = new BitmapImage();
               // productsList.Items.Add(pwp[i].m_ProductName);
                btpImg.UriSource = new Uri(pwp[i].m_ImageURL.Split('|')[0]);

                imgB.Source = btpImg;
                imgB.Height = 100;
                productsList.Items.Add(imgB);
            }
            //// Create a new ListView (or GridView) for the UI, add content by setting ItemsSource

            ImagePanel.Children.Add(productsList);
        }
        #region KNN

        //KNN
        private void KNN()
        {

            foreach (Products elem in pwp)
            {
                double[] ar1 = new double[2];
                Products p = elem;
                p.m_Price = p.m_Price.Replace("$", "");
                //dataset problem with underscores and Total Price
                if (!p.m_Price.Contains('_') && !p.m_Price.Contains('T'))
                {
                    ar1[0] = Convert.ToDouble(elem.m_NumberReviews);
                    ar1[1] = Convert.ToDouble(elem.m_Price);
                    Classify(ar1);
                }
            }
        }
        //"c9b11077922a9749af9a0fba287d4a9a"
        //grab highest points from list and train
        private void Classify(double[] f)
        {
            double min = Double.MaxValue;
            foreach (Products elem in products)
            {
                double[] ar1 = new double[2];
                Products p = elem;
                p.m_Price = p.m_Price.Replace("$", "");
                //dataset problem with underscores and Total Price
                if (!p.m_Price.Contains('_') && !p.m_Price.Contains('T'))
                {
                    if (elem.m_Price == "")
                    {
                        elem.m_Price = "0";
                    }
                    ar1[0] = Convert.ToDouble(elem.m_NumberReviews);
                    ar1[1] = Convert.ToDouble(elem.m_Price);
                }

                double d = Distance(f, ar1);

                if (d < min)
                {
                    min = d;
                    //we can overide the points from here
                    knnl.Add(elem);

                }

            }
        }
        private double Distance(double[] p, double[] q)
        {
            if (p.Length != q.Length)
            {
                throw new Exception("Data are not the same length");
            }

            double d = 0;
            for (int i = 0; i < p.Length; i++)
            {
                d += Math.Pow(p[i] - q[i], 2);
            }

            return Math.Sqrt(d);

        }

        #endregion

        //Point system
        private void AddRelevant()
        {
            CheckName();
            CheckCategory();
        }
        private void CheckName()
        {
            pwp.Clear();
            if (userInput.m_Subject == "baby")
            {
              userInput.m_Noun.Add(userInput.m_Subject);
            }
            if (userInput.m_Value >= 1)
            {


                foreach (Products elem in products)
                {
                    if (pwp.Count >= 50)
                    {
                        break; // for saftey
                    }
                    foreach (string elem2 in userInput.m_Noun)
                    {
                        if (elem.m_ProductName.ToLower().Contains(elem2.ToLower()))
                        {
                            Products p = elem;

                            p.m_Price = p.m_Price.Replace("$", "");
                            if (!p.m_Price.Contains('_') && !p.m_Price.Contains('T'))
                            {
                                if (p.m_Price == "")
                                {
                                    p.m_Price = "0";
                                }
                                p.m_Value = 200 - Convert.ToDouble(elem.m_Price) * userInput.m_Value; // minus price lower the price better the score
                                pwp.Add(p);
                            }

                        }
                    }

                }
            }else if(userInput.m_Value <= -1)
            {
               
                foreach (Products elem in products)
                {
                    if (pwp.Count >= 50)
                    {
                        break; // for saftey
                    }
                    foreach (string elem2 in userInput.m_Noun)
                    {
                        if (!elem.m_ProductName.ToLower().Contains(elem2.ToLower()))
                        {
                            Products p = elem;

                            p.m_Price = p.m_Price.Replace("$", "");
                            if (!p.m_Price.Contains('_') && !p.m_Price.Contains('T'))
                            {
                                if (p.m_Price == "")
                                {
                                    p.m_Price = "0";
                                }
                                p.m_Value = 50 - Convert.ToDouble(elem.m_Price) * userInput.m_Value; // minus price lower the price better the score
                                pwp.Add(p);
                            }

                        }
                    }

                }
            }
        }
        private void CheckCategory()
        {

            if (userInput.m_Value >= 1)
            {

                foreach (Products elem in products)
                {
                    if (pwp.Count >= 100)
                    {
                        break; // for saftey
                    }
                    foreach (string elem2 in userInput.m_Noun)
                    {
                        if (elem.m_Category.ToLower().Contains(elem2.ToLower()))
                        {
                            //increasing the score it already exist
                            if (pwp.Find(x => x.m_ID == elem.m_ID) == elem)
                            {
                                Products p = elem;
                                if (!p.m_Price.Contains('_') && !p.m_Price.Contains('T'))
                                {
                                    p.m_Price = p.m_Price.Replace("$", "");
                                    p.m_Value = 500 - Convert.ToDouble(elem.m_Price) ; // minus price lower the price better the score
                                    pwp.Find(x => x.m_ID == elem.m_ID).m_Value += p.m_Value * userInput.m_Value;
                                }
                            }
                            else
                            {
                                Products p = elem;
                                p.m_Price = p.m_Price.Replace("$", "");
                                //dataset problem with underscores and Total Price
                                if (!p.m_Price.Contains('_') && !p.m_Price.Contains('T'))
                                {
                                    if (p.m_Price == "")
                                    {
                                        p.m_Price = "0";
                                    }
                                    p.m_Value = 500 - Convert.ToDouble(elem.m_Price) * userInput.m_Value; // minus price lower the price better the score
                                    pwp.Add(p);
                                }

                            }
                        }
                    }

                }
            }else if (userInput.m_Value <= -1)
            {
                foreach (Products elem in products)
                {
                    if (pwp.Count >= 100)
                    {
                        break; // for saftey
                    }
                    foreach (string elem2 in userInput.m_Noun)
                    {
                        if (!elem.m_Category.ToLower().Contains(elem2.ToLower()))
                        {
                            //increasing the score it already exist
                            if (pwp.Find(x => x.m_ID == elem.m_ID) == elem)
                            {
                                Products p = elem;
                                if (!p.m_Price.Contains('_') && !p.m_Price.Contains('T'))
                                {
                                    p.m_Price = p.m_Price.Replace("$", "");
                                    p.m_Value = 50 - Convert.ToDouble(elem.m_Price); // minus price lower the price better the score
                                    pwp.Find(x => x.m_ID == elem.m_ID).m_Value += p.m_Value * userInput.m_Value;
                                }
                            }
                            else
                            {
                                Products p = elem;
                                p.m_Price = p.m_Price.Replace("$", "");
                                //dataset problem with underscores and Total Price
                                if (!p.m_Price.Contains('_') && !p.m_Price.Contains('T'))
                                {
                                    if (p.m_Price == "")
                                    {
                                        p.m_Price = "0";
                                    }
                                    p.m_Value = 50 - Convert.ToDouble(elem.m_Price) * userInput.m_Value; // minus price lower the price better the score
                                    pwp.Add(p);
                                }

                            }
                        }
                    }

                }
            }
        }
    }
}
