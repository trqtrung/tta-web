using System;
using System.Linq;
using TTA.Api.Models;

namespace TTA.Api.Data
{
    public class DbInitialize
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            //check if there is any client option
            if (context.Products.Any())
            {
                //return;
            }
            else
            {
                var products = new Product[]
                {
                new Product{ Name="Kềm nhọn 7-inch Licota", Description="Licota Long Nose Plier 7-inch", Created = DateTime.Now},
                new Product{ Name="Kềm cắt 7-inch Licota", Description="Licota Heavy Cutter Plier 7-inch", Created = DateTime.Now},
                new Product{ Name="Kềm điện 7-inch Licota", Description="Licota Combination Plier 7-inch", Created=DateTime.Now}
                };

                foreach (Product p in products)
                {
                    context.Products.Add(p);
                }
                context.SaveChanges();
            }

            //brand
            //check if there is any client option
            if (context.Brands.Any())
            {
                //return;
            }
            else
            {
                var brands = new Brand[]
                {

                    new Brand { Name = "No brand", Description = "no-brand", Created = DateTime.Now },
                new Brand { Name = "TOP", Description = "TOP Taiwan", Created = DateTime.Now },
                new Brand { Name = "Licota", Description = "Licota Taiwan", Created = DateTime.Now },
                new Brand { Name = "Century", Description = "Century Taiwan", Created = DateTime.Now },
                new Brand { Name = "Asaki", Description = "Asaki China Japan Techology", Created = DateTime.Now },
                new Brand { Name = "Berrylion", Description = "Berrylion China", Created = DateTime.Now },
                new Brand { Name = "Wynn's", Description = "Wynn's China", Created = DateTime.Now },
                new Brand { Name = "Barker", Description = "Barker China", Created = DateTime.Now },
                new Brand { Name = "Master", Description = "Master USA", Created = DateTime.Now },
                new Brand { Name = "Việt Tiệp", Description = "Việt Tiệp Việt Nam", Created = DateTime.Now },
                new Brand { Name = "Yeti", Description = "Yeti Taiwan", Created = DateTime.Now },
                new Brand { Name = "LS", Description = "LS Việt Nam", Created = DateTime.Now },
                new Brand { Name = "Onika", Description = "Onika China", Created = DateTime.Now },
                new Brand { Name = "NRT", Description = "NRT China", Created = DateTime.Now },
                new Brand { Name = "Namilux", Description = "Namilux Việt Nam", Created = DateTime.Now },
                new Brand { Name = "Vinachi", Description = "Việt Nam", Created = DateTime.Now },
                new Brand { Name = "Alex", Description = "Alex China", Created = DateTime.Now },
                new Brand { Name = "Black Hand", Description = "Black Hand Taiwan", Created = DateTime.Now },
                new Brand { Name = "Allpro", Description = "Allpro Taiwan", Created = DateTime.Now },
                new Brand { Name = "Solex", Description = "Solex Thailand", Created = DateTime.Now },
                new Brand { Name = "Makita", Description = "Makita China", Created = DateTime.Now },
                new Brand { Name = "Standard", Description = "China", Created = DateTime.Now },
                new Brand { Name = "Dudaco", Description = "Dudaco Việt Nam", Created = DateTime.Now },
                new Brand { Name = "Vuong Niem", Description = "Vuong Niem Viet Nam", Created = DateTime.Now },
                new Brand { Name = "Junxo", Description = "Junxo India", Created = DateTime.Now },
                new Brand { Name = "Luxtop", Description = "Luxtop China", Created = DateTime.Now },
                new Brand { Name = "YCU-II", Description = "YCU-II Hà Nội", Created = DateTime.Now },
            };

                foreach (Brand item in brands)
                {
                    context.Brands.Add(item);
                }
                context.SaveChanges();
            }




            //check if there is any product record
            if (context.OptionLists.Any())
            {
                //return;
            }
            else
            {
                var options = new OptionList[]
            {
                //e-commerce websites
                new OptionList{ Name="Lazada", Key="e-commerce-client", Value="lazada"},
                new OptionList{ Name="Shopee", Key="e-commerce-client",Value="shopee"},
                new OptionList{ Name="Sendo", Key="e-commerce-client",Value="sendo"},

                //shipping services
                new OptionList{ Name="Giao hàng nhanh (GHN)", Key="shipping-service",Value="ghn"},
                new OptionList{ Name="Giao hàng tiết kiệm (GHTK)", Key="shipping-service",Value="ghtk"},
                new OptionList{ Name="DHL", Key="shipping-service",Value="dhl"},
                new OptionList{ Name="Ninja Van", Key="shipping-service",Value="ninjavan"},

                //payment methods
                new OptionList{ Name="Cash on Delivery (COD)", Key="payment-method",Value="cod"},
                new OptionList{ Name="Cash", Key="payment-method",Value="cash"},
                new OptionList{ Name="Transfer", Key="payment-method",Value="transfer"},
                new OptionList{ Name="VISA", Key="payment-method",Value="visa"},

                //order stages
                new OptionList{ Name="New", Key="order-stage",Value="new"},
                new OptionList{ Name="Waiting for Confirmation", Key="order-stage",Value="waiting-confirm"},
                new OptionList{ Name="Confirmed", Key="order-stage",Value="confirmed"},
                new OptionList{ Name="Ready for Shipping", Key="order-stage",Value="ready-shipping" },
                new OptionList{ Name="Shipping", Key="order-stage",Value="shipping"},
                new OptionList{ Name="Delivered", Key="order-stage",Value="delivered"},
                new OptionList{ Name="Waiting for Payment", Key="order-stage",Value="waiting-payment"},
                new OptionList{ Name="Completed", Key="order-stage",Value="completed"},
                new OptionList{ Name="Cancelled", Key="order-stage",Value="cancelled"},
                new OptionList{ Name="Return", Key="order-stage",Value="return"},

                //order tracking types
                new OptionList{ Name="Shipping", Key="order-tracking-type",Value="shipping"},
                //new OptionList{ Name="Payment", Key="order-tracking-type",Value="payment"},
                new OptionList{ Name="Stage", Key="order-tracking-type",Value="stage"},

                //order shipping stages
                new OptionList{ Name="Ready for Shipping", Key="order-shipping",Value="ready-shipping"},
                new OptionList{ Name="Picked Up", Key="order-shipping",Value="picked"},
                new OptionList{ Name="Shipping", Key="order-shipping",Value="shipping"},
                new OptionList{ Name="Lost", Key="order-shipping",Value="lost"},
                new OptionList{ Name="Delivered", Key="order-shipping",Value="delivered"},
                new OptionList{ Name="Return", Key="order-shipping",Value="return"},

                //order types
                new OptionList{ Name="Khóa cửa cố định", Key="product-type",Value="lock"},
                new OptionList{ Name="Dụng cụ đo lường", Key="product-type",Value="measurement"},
                new OptionList{ Name="Đồ dùng văn phòng", Key="product-type",Value="office"},
                new OptionList{ Name="Thiết bị phòng ăn", Key="product-type",Value="kitchen"},
                new OptionList{ Name="Thiết bị phòng tắm", Key="product-type",Value="bathroom"},
                new OptionList{ Name="Sản phẩm khác", Key="product-type",Value="others"},
                new OptionList{ Name="Bộ đồ nghề sửa chữa", Key="product-type",Value="diy"},
                new OptionList{ Name="Bảo hộ lao động", Key="product-type",Value="protection"},
                new OptionList{ Name="Báo động & chống trộm", Key="product-type",Value="security"},
                new OptionList{ Name="Phụ kiện Ô tô - Xe máy - Xe đạp", Key="product-type",Value="vehicle"},
                new OptionList{ Name="Dụng cụ sân vườn", Key="product-type",Value="garden"},
                new OptionList{ Name="Thiết bị điện & phụ kiện", Key="product-type",Value="electronic"},

                new OptionList{ Name="Khóa lục giác", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Kềm", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Dụng cụ tháo lắp", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Kéo & Nhíp", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Bộ dao", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Đinh vít & Ốc vít", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Vặn đai ốc", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Phụ kiện dụng cụ cầm tay", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Cưa", Key="product-type-sub",Value=string.Empty},                
                new OptionList{ Name="Súng bắn keo", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Súng thổi hơi nóng", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Bấm đinh & ghim", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Phụ kiện Ô tô", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Phụ kiện xe máy", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Phụ kiện xe đạp", Key="product-type-sub",Value=string.Empty},                
                new OptionList{ Name="Máy khoan", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Bắt diệt côn trùng", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Keo", Key="product-type-sub",Value=string.Empty},
                new OptionList{ Name="Búa", Key="product-type-sub",Value=string.Empty},

            };

                foreach (OptionList o in options)
                {
                    context.OptionLists.Add(o);
                }
                context.SaveChanges();
            }

            
        }
    }
}
