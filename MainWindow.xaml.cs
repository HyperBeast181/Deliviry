using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;   
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;



namespace Deliviry
{
    public partial class MainWindow : Window
    {
        DeliviryEntities db;



        public MainWindow()
        {
            InitializeComponent();
            db = new DeliviryEntities();
            LoadData();

        }
        
        private void LoadData()
        {
            try
            {

                db.Restaurants.Load();
                ListViewRestaurants.ItemsSource = db.Restaurants.Local;

                db.Dishes.Load();
                ListViewDishes.ItemsSource = db.Dishes.Local;

                db.Orders.Load();
                ListViewOrders.ItemsSource = db.Orders.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var restaurant = new Restaurants
                {
                    Name = NameTextBox.Text,
                    Address = AddressTextBox.Text,
                    Rating = int.TryParse(RatingTextBox.Text, out int rating) ? rating : 0 
                };

                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                LoadData();
                ListViewRestaurants.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении ресторана: {ex.Message}");
            }

        }

        private Restaurants selectedRestaurant;

        private void ListViewRestaurants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewRestaurants.SelectedItem != null)
            {
                selectedRestaurant = (Restaurants)ListViewRestaurants.SelectedItem;
                NameTextBox.Text = selectedRestaurant.Name;
                AddressTextBox.Text = selectedRestaurant.Address;
                RatingTextBox.Text = selectedRestaurant.Rating.ToString();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRestaurant != null)
            {
                selectedRestaurant.Name = NameTextBox.Text;
                selectedRestaurant.Address = AddressTextBox.Text;
                selectedRestaurant.Rating = int.Parse(RatingTextBox.Text);

                db.Entry(selectedRestaurant).State = EntityState.Modified;
                db.SaveChanges();
                LoadData();
                ListViewRestaurants.Items.Refresh();

            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRestaurant != null && MessageBox.Show("Вы уверены, что хотите удалить этот ресторан?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.Restaurants.Remove(selectedRestaurant);
                db.SaveChanges();
                LoadData();
                ListViewRestaurants.Items.Refresh();

            }
        }

        private Dishes selectedDish;


        private void AddDishButton_Click(object sender, RoutedEventArgs e)
        {
            var dish = new Dishes
            {
                Name = NameTextBox1.Text,
                Description = opisanie.Text,
                Price = decimal.Parse(value.Text)
            };

            db.Dishes.Add(dish);
            db.SaveChanges();
            LoadData(); 
            ListViewDishes.Items.Refresh();

        }

        private void EditDishButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDish != null)
            {
                selectedDish.Name = NameTextBox1.Text;
                selectedDish.Description = opisanie.Text;
                selectedDish.Price = decimal.Parse(value.Text);

                db.Entry(selectedDish).State = EntityState.Modified;
                db.SaveChanges();
                LoadData();
                ListViewDishes.Items.Refresh();

            }
        }

        private void DeleteDishButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDish != null && MessageBox.Show("Вы уверены, что хотите удалить это блюдо?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.Dishes.Remove(selectedDish);
                db.SaveChanges();
                LoadData(); 
                ListViewDishes.Items.Refresh();

            }
        }

        private void ListViewDishes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewDishes.SelectedItem != null)
            {
                selectedDish = (Dishes)ListViewDishes.SelectedItem;
                NameTextBox1.Text = selectedDish.Name;
                opisanie.Text = selectedDish.Description;
                value.Text = selectedDish.Price.ToString();
            }
        }


        private Orders selectedOrder;

        // Обработчик добавления заказа
        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var order = new Orders
            {
                CustomerName = Client.Text,
                OrderTime = DateTime.Parse(time.Text) 
            };

            db.Orders.Add(order);
            db.SaveChanges();
            LoadData(); 
            ListViewOrders.Items.Refresh();
        }

        private void ListViewOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewOrders.SelectedItem != null)
            {
                selectedOrder = (Orders)ListViewOrders.SelectedItem;
                Client.Text = selectedOrder.CustomerName;
                time.Text = selectedOrder.OrderTime.ToString();
            }
        }

        private void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOrder != null && MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                db.Orders.Remove(selectedOrder);
                db.SaveChanges();
                LoadData(); 
                ListViewOrders.Items.Refresh();
            }
        }




    }
}




