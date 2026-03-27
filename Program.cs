using System;
using System.Collections.Generic;

interface IEntity
{
    int Id { get; set; }
}

interface IRepository<T> where T : IEntity
{
    void Add(T item);
    void Delete(T item);
    T FindById(int id);
    IEnumerable<T> GetAll();
}

class Product : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(int id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}

class Customer : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

    public Customer(int id, string name, string address)
    {
        Id = id;
        Name = name;
        Address = address;
    }
}

class ProductRepository : IRepository<Product>
{
    private List<Product> items = new List<Product>();

    public void Add(Product item)
    {
        items.Add(item);
    }

    public void Delete(Product item)
    {
        items.Remove(item);
    }

    public Product FindById(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
                return items[i];
        }

        throw new Exception("Product не найден");
    }

    public IEnumerable<Product> GetAll()
    {
        return items;
    }
}

class CustomerRepository : IRepository<Customer>
{
    private List<Customer> items = new List<Customer>();

    public void Add(Customer item)
    {
        items.Add(item);
    }

    public void Delete(Customer item)
    {
        items.Remove(item);
    }

    public Customer FindById(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
                return items[i];
        }

        throw new Exception("Customer не найден");
    }

    public IEnumerable<Customer> GetAll()
    {
        return items;
    }
}

interface IClonable<T> where T : class
{
    T Clone();
}

class Point : IClonable<Point>
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point(Point other)
    {
        X = other.X;
        Y = other.Y;
    }

    public Point Clone()
    {
        return new Point(this);
    }
}

class Rectangle : IClonable<Rectangle>
{
    public Point LeftTop { get; set; }
    public Point RightBottom { get; set; }

    public Rectangle(Point leftTop, Point rightBottom)
    {
        LeftTop = leftTop;
        RightBottom = rightBottom;
    }

    public Rectangle(Rectangle other)
    {
        LeftTop = new Point(other.LeftTop);
        RightBottom = new Point(other.RightBottom);
    }

    public Rectangle Clone()
    {
        return new Rectangle(this);
    }
}

struct ComplexNumber : IComparable<ComplexNumber>
{
    public double Real { get; set; }
    public double Imaginary { get; set; }

    public ComplexNumber(double real, double imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    public int CompareTo(ComplexNumber y)
    {
        double m1 = Math.Sqrt(Real * Real + Imaginary * Imaginary);
        double m2 = Math.Sqrt(y.Real * y.Real + y.Imaginary * y.Imaginary);

        return m1.CompareTo(m2);
    }

    public override string ToString()
    {
        return Real + " + " + Imaginary + "i";
    }
}

struct RationalNumber : IComparable<RationalNumber>
{
    public int Numerator { get; set; }
    public int Denominator { get; set; }

    public RationalNumber(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }

    public int CompareTo(RationalNumber y)
    {
        int left = Numerator * y.Denominator;
        int right = y.Numerator * Denominator;

        return left.CompareTo(right);
    }

    public override string ToString()
    {
        return Numerator + "/" + Denominator;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("1. IRepository<T>");

        ProductRepository productRepository = new ProductRepository();
        productRepository.Add(new Product(1, "Ноутбук", 55000));
        productRepository.Add(new Product(2, "Мышь", 1500));

        CustomerRepository customerRepository = new CustomerRepository();
        customerRepository.Add(new Customer(1, "Иван", "Москва"));
        customerRepository.Add(new Customer(2, "Анна", "Казань"));

        Product product = productRepository.FindById(1);
        Console.WriteLine("Найден продукт: " + product.Name + ", " + product.Price);

        Customer customer = customerRepository.FindById(2);
        Console.WriteLine("Найден клиент: " + customer.Name + ", " + customer.Address);

        Console.WriteLine("Все продукты:");
        foreach (Product item in productRepository.GetAll())
        {
            Console.WriteLine(item.Id + " " + item.Name + " " + item.Price);
        }

        Console.WriteLine("Все клиенты:");
        foreach (Customer item in customerRepository.GetAll())
        {
            Console.WriteLine(item.Id + " " + item.Name + " " + item.Address);
        }

        Console.WriteLine();
        Console.WriteLine("2. IClonable<T>");

        Point p1 = new Point(10, 20);
        Point p2 = MakeClone(p1);

        Rectangle r1 = new Rectangle(new Point(0, 0), new Point(100, 50));
        Rectangle r2 = MakeClone(r1);

        Console.WriteLine("Point: " + p1.X + ", " + p1.Y);
        Console.WriteLine("Clone Point: " + p2.X + ", " + p2.Y);

        Console.WriteLine("Rectangle:");
        Console.WriteLine("LeftTop: " + r1.LeftTop.X + ", " + r1.LeftTop.Y);
        Console.WriteLine("RightBottom: " + r1.RightBottom.X + ", " + r1.RightBottom.Y);

        Console.WriteLine("Clone Rectangle:");
        Console.WriteLine("LeftTop: " + r2.LeftTop.X + ", " + r2.LeftTop.Y);
        Console.WriteLine("RightBottom: " + r2.RightBottom.X + ", " + r2.RightBottom.Y);

        Console.WriteLine();
        Console.WriteLine("3. IComparable<T>");

        ComplexNumber c1 = new ComplexNumber(3, 4);
        ComplexNumber c2 = new ComplexNumber(1, 1);

        int complexResult = c1.CompareTo(c2);
        Console.WriteLine("Сравнение комплексных чисел: " + complexResult);

        RationalNumber q1 = new RationalNumber(1, 2);
        RationalNumber q2 = new RationalNumber(3, 4);

        int rationalResult = q1.CompareTo(q2);
        Console.WriteLine("Сравнение рациональных чисел: " + rationalResult);

        Console.ReadLine();
    }

    static T MakeClone<T>(T obj) where T : class, IClonable<T>
    {
        return obj.Clone();
    }
}