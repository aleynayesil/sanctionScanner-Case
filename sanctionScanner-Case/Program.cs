using HtmlAgilityPack;
using sanctionScanner_Case.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace sanctionScanner_Case
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sahibinden.com anasayfa vitrinden çekilen veriler ekrana yazdırıldı.
            //Veri çekilirken Html Agility Pack paketi kullanıldı.
            //Ancak çok istek sebebiyle site tarafından engellendim. Bu sebeple sadece bir kez veri çekilip ortalama hesaplanabiliyor.


            var homeShowCase = new List<HomeShowCase>(); //Gösterilecek veri listesi

            var url = @"https://www.sahibinden.com";//Verinin çekileceği Url adresi

            HtmlWeb web = new HtmlWeb();//HtmlWeb yapısını kullanabilmek için HtmlAgilityPack paketi indirildi.

            var htmlDoc = web.Load(url);//htmldocument oluşturuyoruz.

            var node = htmlDoc.DocumentNode.SelectNodes("//div[@class='uiBox showcase']");//İlgili html alanını kapsayan html etiketi 

            foreach (HtmlAgilityPack.HtmlNode listItem in node)
            {
                foreach (var innerItem in listItem.SelectNodes("ul//li"))//her ul un içindeki li de dön

                {
                    var attr = url + innerItem.SelectSingleNode("a").Attributes["href"].Value;//detay sayfası için link alındı

                    //HtmlWeb web2 = new HtmlWeb();

                    var htmlDetailDoc = web.Load(attr);

                    var nodeDetail = htmlDetailDoc.DocumentNode.SelectSingleNode("/html/body/div[5]/div[4]/div[1]/div/div[2]/div[2]/h3/text()");//detay sayfasındaki fiyat bilgisi alındı

                    if (nodeDetail != null)//çekilen verilerden reklam geldiğinde null geldiğinden reklam olmayan verileri alındı
                    {
                        var currentPrice = nodeDetail.InnerHtml.Trim();
                        var price = (Regex.Replace(currentPrice, "[^0-9]", ""));//htmlden alınan price bilgisinini sadece rakam olarak alır

                        var adTitle = innerItem.SelectSingleNode("a//span").InnerHtml;

                        var list = new HomeShowCase()
                        {
                            AdvertName = adTitle.Trim(),
                            Price = Convert.ToDecimal(price)
                        };

                        homeShowCase.Add(list);

                    }
                    else
                    {
                        continue;//reklam gelirse bir sonraki veriye geçer
                    }

                }

            }
            decimal total = 0;
            foreach (var item in homeShowCase)
            {
                Console.WriteLine(item.AdvertName + " " + item.Price);
                total += item.Price;

            }
            var avg = total / homeShowCase.Count();
            Console.WriteLine("Ortalama Fiyat: " + Math.Round(avg, 2));

            //txt dosyası oluşturma
            using (StreamWriter writer = new StreamWriter(@"C:\Users\aleynayesil\source\repos\sanctionScanner-Case\sanctionScanerResult.txt"))
            {
                foreach (var list in homeShowCase)
                {
                    writer.WriteLine(list.AdvertName + " " + list.Price);
                }
                writer.WriteLine();
                writer.WriteLine("Ortalama Fiyat : " + Math.Round(avg, 2));
            }
        }
    }
}
