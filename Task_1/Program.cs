using System;
using System.Collections.Generic;
using System.Linq;

// Клас Товар
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public double Rating { get; set; }

    public Product(string name, decimal price, string description, string category, double rating)
    {
        Name = name;
        Price = price;
        Description = description;
        Category = category;
        Rating = rating;
    }

    public override string ToString()
    {
        return $"Товар: {Name}, Ціна: {Price} грн, Категорія: {Category}, Рейтинг: {Rating}";
    }
}

// Клас Користувач
public class User
{
    public string Login { get; set; }
    public string Password { get; set; }
    public List<Order> PurchaseHistory { get; set; }

    public User(string login, string password)
    {
        Login = login;
        Password = password;
        PurchaseHistory = new List<Order>();
    }

    public void AddOrderToHistory(Order order)
    {
        PurchaseHistory.Add(order);
    }
}

// Клас Замовлення
public class Order
{
    public List<Product> Products { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }

    public Order(List<Product> products, int quantity)
    {
        Products = products;
        Quantity = quantity;
        TotalAmount = products.Sum(p => p.Price * quantity);
        Status = "Очікує обробки";
    }

    public override string ToString()
    {
        return $"Замовлення: {Products.Count} товарів, Загальна сума: {TotalAmount} грн, Статус: {Status}";
    }
}

// Інтерфейс для пошуку
public interface ISearchable
{
    List<Product> SearchProducts(string criteria, object value);
}

// Клас Магазин
public class Store : ISearchable
{
    private List<Product> products = new List<Product>();
    private List<User> users = new List<User>();
    private List<Order> orders = new List<Order>();

    public void AddProduct(Product product)
    {
        products.Add(product);
    }

    public void RegisterUser(User user)
    {
        users.Add(user);
    }

    public void PlaceOrder(User user, Order order)
    {
        orders.Add(order);
        user.AddOrderToHistory(order);
        Console.WriteLine("Замовлення успішно оформлено!");
    }

    public List<Product> SearchProducts(string criteria, object value)
    {
        return criteria.ToLower() switch
        {
            "price" => products.Where(p => p.Price <= (decimal)value).ToList(),
            "category" => products.Where(p => p.Category.Equals((string)value, StringComparison.OrdinalIgnoreCase)).ToList(),
            "rating" => products.Where(p => p.Rating >= (double)value).ToList(),
            _ => new List<Product>()
        };
    }

    public void DisplayProducts()
    {
        foreach (var product in products)
        {
            Console.WriteLine(product);
        }
    }
}

// Тестування
class Program
{
    static void Main(string[] args)
    {
        // Створення магазину
        var store = new Store();

        // Додавання товарів
        store.AddProduct(new Product("Ноутбук", 25000, "Потужний ноутбук", "Електроніка", 4.5));
        store.AddProduct(new Product("Телефон", 15000, "Смартфон", "Електроніка", 4.7));
        store.AddProduct(new Product("Холодильник", 12000, "Енергозберігаючий", "Побутова техніка", 4.2));

        // Реєстрація користувача
        var user = new User("user1", "password123");
        store.RegisterUser(user);

        // Відображення товарів
        Console.WriteLine("Список доступних товарів:");
        store.DisplayProducts();

        // Пошук товарів
        Console.WriteLine("\nПошук товарів за категорією 'Електроніка':");
        var electronics = store.SearchProducts("category", "Електроніка");
        electronics.ForEach(Console.WriteLine);

        // Оформлення замовлення
        var order = new Order(electronics, 1);
        store.PlaceOrder(user, order);

        // Історія покупок користувача
        Console.WriteLine("\nІсторія покупок користувача:");
        user.PurchaseHistory.ForEach(Console.WriteLine);
    }
}
