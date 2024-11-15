using BitcoinPriceViewer.Models;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace BitcoinPriceViewer
{
    public partial class Market : Window
    {
        private readonly RedisClient _redisClient;
        private readonly string _redisListName = "crypto-prices";
        private readonly DispatcherTimer _timer;

        public Market()
        {
            InitializeComponent();

            // Kết nối Redis
            _redisClient = new RedisClient("127.0.0.1", 6379);

            // Khởi tạo timer để cập nhật dữ liệu
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            // Lấy dữ liệu ban đầu
            UpdateCryptoData();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateCryptoData();
        }

        private void UpdateCryptoData()
        {
            // Lấy bản ghi mới nhất từ Redis
            string latestPriceJson = _redisClient.GetItemFromList(_redisListName, -1);

            if (string.IsNullOrEmpty(latestPriceJson))
            {
                return;
            }

            // Parse JSON thành dictionary chứa các cặp Name-Price
            var prices = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(latestPriceJson);

            if (prices == null) return;

            // Chuyển đổi dictionary thành danh sách các đối tượng CryptoCurrency
            var cryptoData = prices.Select(p => new CryptoCurrency {
                Name = p.Key,
                LastPrice = p.Value 
            }).ToList();

            // Cập nhật dữ liệu cho DataGrid
            Dispatcher.Invoke(() =>
            {
                CryptoDataGrid.ItemsSource = cryptoData;
            });
        }
    }
}
