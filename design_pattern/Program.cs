using System;
using System.Collections.Generic;
using System.Text;

// Singleton
class Restaurant
{
    private static Restaurant? _instance;
    private readonly List<MonAnMoi> _orders = [];

    private Restaurant()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("---------------chào mừng bạn đã đến nhà hàng của Goài Xơn---------------");
        Console.ResetColor();
    }

    public static Restaurant Instance()
    {
        return _instance ??= new Restaurant();
    }

    public void OrderFood(MonAnMoi food)
    {
        _orders.Add(food);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Đã thêm vào order: {food}");
        Console.ResetColor();
    }

    public void ShowOrders()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nDanh sách món ăn đã order:");
        int soThuTu = 0;
        foreach (var item in _orders)
        {
            Console.WriteLine($"[{soThuTu++}] - {item}");
        }
        Console.ResetColor();
    }

    public MonAnMoi GetOrder(int index)
    {
        if (_orders.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Chưa có món nào được gọi mom ạ, order trước nhé!");
            Console.ResetColor();
            return null!;
        }
        if (index >= 0 && index < _orders.Count)
        {
            return _orders[index];
        }
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("THÔI LƯỚT!");
        Environment.Exit(1);
        Console.ResetColor();
        return null!;
    }
}

// Prototype
abstract class MonAn : ICloneable
{
    public string? tenMonAn;
    public string? moTa;
    public string? nguyenLieu;

    public abstract object Clone();
    public override string ToString() => $"Tên món ăn: {tenMonAn}, Mô tả: {moTa}, Nguyên liệu: {nguyenLieu}";
}

class MonAnMoi : MonAn
{
    public override object Clone() => (MonAnMoi)this.MemberwiseClone();
}

// Builder
class DishBuilder
{
    private readonly MonAnMoi _monAn;

    public DishBuilder()
    {
        _monAn = new MonAnMoi();
    }

    public void SetTenMonAn(string name)
    {
        _monAn.tenMonAn = name;
    }

    public void SetMoTa(string description)
    {
        _monAn.moTa = description;
    }

    public void SetNguyenLieu(string nguyenLieu)
    {
        _monAn.nguyenLieu = nguyenLieu;
    }

    public MonAnMoi Build()
    {
        return _monAn;
    }
}

// Factory
interface IMonAnFactory
{
    MonAnMoi CreateHuTieu();
    MonAnMoi CreateMi();
    MonAnMoi CreatePho();
}

class ChickenFactory : IMonAnFactory
{
    public MonAnMoi CreateHuTieu() => new() { tenMonAn = "Hủ Tiếu Gà", moTa = "Hủ tiếu với thịt gà" };
    public MonAnMoi CreateMi() => new() { tenMonAn = "Mì Gà", moTa = "Mì với thịt gà" };
    public MonAnMoi CreatePho() => new() { tenMonAn = "Phở Gà", moTa = "Phở với thịt gà" };
}

class BeefFactory : IMonAnFactory
{
    public MonAnMoi CreateHuTieu() => new() { tenMonAn = "Hủ Tiếu Bò", moTa = "Hủ tiếu với thịt bò" };
    public MonAnMoi CreateMi() => new() { tenMonAn = "Mì Bò", moTa = "Mì với thịt bò" };
    public MonAnMoi CreatePho() => new() { tenMonAn = "Phở Bò", moTa = "Phở với thịt bò" };
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Restaurant restaurant = Restaurant.Instance();
        bool finish = false;

        do
        {
            int choice = Menu();
            switch (choice)
            {
                case 1:
                    OrderTuMenu(restaurant);
                    break;
                case 2:
                    restaurant.OrderFood(BuildMotMonAn());
                    break;
                case 3:
                    CopyMonAn(restaurant);
                    break;
                case 4:
                    CheckBill(restaurant);
                    break;
                case 5:
                    finish = ExitApp();
                    break;
                default:
                    Console.WriteLine("Chức năng không tồn tại");
                    break;
            }
        } while (!finish);

        restaurant.ShowOrders();
    }

    private static int Menu()
    {
        Console.WriteLine("-------------------Chọn chức năng-------------------");
        Console.WriteLine("1. Tiểu nhị, cho xin cái menu");
        Console.WriteLine("2. Để ta tự vào bếp nấu");
        Console.WriteLine("3. Cho bổn cung copy 1 món");
        Console.WriteLine("4. Check bill");
        Console.WriteLine("5. Thoát");
        ShowChoosenOption();
        _ = int.TryParse(Console.ReadLine(), out int choice);
        return choice;
    }

    private static void OrderTuMenu(Restaurant restaurant)
    {
        IMonAnFactory beefFactory = new BeefFactory();
        IMonAnFactory chickenFactory = new ChickenFactory();

        Console.WriteLine("Nhà hàng của tại hạ có các món sau đây:");
        while (true)
        {
            Console.WriteLine("1. Phở Bò\n2. Mì Bò\n3. Hủ Tiếu Bò\n4. Phở Gà\n5. Mì Gà\n6. Hủ Tiếu Gà\n0. Chọn xong rồi!");
            ShowChoosenOption();
            _ = int.TryParse(Console.ReadLine(), out int choice);
            switch (choice)
            {
                case 1:
                    restaurant.OrderFood(beefFactory.CreatePho());
                    break;
                case 2:
                    restaurant.OrderFood(beefFactory.CreateMi());
                    break;
                case 3:
                    restaurant.OrderFood(beefFactory.CreateHuTieu());
                    break;
                case 4:
                    restaurant.OrderFood(chickenFactory.CreatePho());
                    break;
                case 5:
                    restaurant.OrderFood(chickenFactory.CreateMi());
                    break;
                case 6:
                    restaurant.OrderFood(chickenFactory.CreateHuTieu());
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Món ăn không tồn tại, vui lòng chọn lại!");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Gọi thêm món nữa khum?");
            Console.ResetColor();
        }
    }

    private static MonAnMoi BuildMotMonAn()
    {
        while (true)
        {
            Console.Write("Nhập tên món ăn:");
            string? tenMonAn = Console.ReadLine();
            if (string.IsNullOrEmpty(tenMonAn))
            {
                Console.WriteLine("Tên món ăn không được để trống, vui lòng nhập lại!");
                continue;
            }

            Console.Write("Nhập mô tả món ăn:");
            string? moTa = Console.ReadLine();
            if (string.IsNullOrEmpty(moTa))
            {
                Console.WriteLine("Mô tả món ăn không được để trống, vui lòng nhập lại!");
                continue;
            }

            Console.Write("Nhập nguyên liệu món ăn:");
            string? nguyenLieu = Console.ReadLine();
            if (string.IsNullOrEmpty(nguyenLieu))
            {
                Console.WriteLine("Nguyên liệu món ăn không được để trống, vui lòng nhập lại!");
                continue;
            }

            Console.Write("Món ăn đã nhập:");
            Console.Write($"Tên: {tenMonAn}, Mô tả: {moTa}, Nguyên liệu: {nguyenLieu}");

            return new MonAnMoi { tenMonAn = tenMonAn, moTa = moTa, nguyenLieu = nguyenLieu };
        }
    }

    private static void CopyMonAn(Restaurant ordered)
    {
        ordered.ShowOrders();
        var check = ordered.GetOrder(0);
        if (check != null)
        {
            Console.WriteLine("Chọn món ăn cần copy:");
            ShowChoosenOption();
            _ = int.TryParse(Console.ReadLine(), out int choice);
            MonAnMoi clonedDish = (MonAnMoi)ordered.GetOrder(choice).Clone();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"Món ăn đã clone: ");
            ordered.OrderFood(clonedDish);
            Console.ResetColor();

        }
    }

    private static void CheckBill(Restaurant restaurant)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        restaurant.ShowOrders();
        Console.ResetColor();
    }

    private static bool ExitApp()
    {
        return true;
    }

    private static void ShowChoosenOption()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("Chọn chức năng số: ");
        Console.ResetColor();
    }
}

//git checkout -b design_pattern