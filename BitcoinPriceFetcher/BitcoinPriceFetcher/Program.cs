using Newtonsoft.Json;
using ServiceStack.Redis;
using System.Net.Http;
using System.Threading;
using System;
using System.Collections.Generic;
using BitcoinPriceFetcher.Models;

namespace BitcoinPriceFetcher
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            // Kết nối Redis
            var redisManager = new RedisManagerPool("redis:6379");
            using (var client = redisManager.GetClient())
            {
                // Tên list Redis
                string redisListName = "crypto-prices";

                // Client HTTP để gọi API
                using var httpClient = new HttpClient();

                while (true)
                {
                    try
                    {
                        // Danh sách các đồng tiền điện tử và API tương ứng
                        var symbols = new List<string>
                        {
                            "BTCUSDT", "ETHUSDT", "SOLUSDT", "BNBUSDT", "DOGEUSDT",
                            "XRPUSDT", "ADAUSDT", "SHIBUSDT", "TRXUSDT", "AVAXUSDT",
                            "TONUSDT", "WBTCUSDT", "LINKUSDT", "BCHUSDT", "SUIUSDT",
                            "DOTUSDT", "APTUSDT", "NEARUSDT", "LTCUSDT", "WBETHUSDT",
                            "PEPEUSDT", "UNIUSDT", "DAIUSDT", "ICPUSDT", "TAOUSDT",
                            "RENDERUSDT", "ETCUSDT", "FETUSDT", "POLUSDT", "STXUSDT",
                            "BONKUSDT", "AAVEUSDT", "INJUSDT", "ARBUSDT", "FILUSDT",
                            "FDUSDUSDT", "HBARUSDT", "TIAUSDT", "IMXUSDT", "FLOKIUSDT",
                            "VETUSDT", "FTMUSDT", "OPUSDT", "ATOMUSDT", "RUNEUSDT",
                            "SEIUSDT", "GRTUSDT", "ENAUSDT", "PNUTUSDT", "JUPUSDT",
                            "WLDUSDT", "PYTHUSDT", "THETAUSDT", "MKRUSDT", "RAYUSDT",
                            "ALGOUSDT"
                        };

                        // Lưu trữ kết quả thông tin của từng đồng tiền
                        var cryptoPrices = new Dictionary<string, CryptoCurrency>();

                        // Lấy thông tin cho từng đồng tiền
                        foreach (var symbol in symbols)
                        {
                            var response = await httpClient.GetAsync($"https://api.binance.com/api/v3/ticker/24hr?symbol={symbol}");
                            response.EnsureSuccessStatusCode();
                            var content = await response.Content.ReadAsStringAsync();
                            dynamic data = JsonConvert.DeserializeObject(content);

                            // Trích xuất thông tin từ API và sử dụng TryParseDecimal để xử lý null
                            decimal lastPrice = TryParseDecimal(data.lastPrice);
                            decimal priceChange = TryParseDecimal(data.priceChange);
                            decimal priceChangePercent = TryParseDecimal(data.priceChangePercent);
                            decimal weightedAvgPrice = TryParseDecimal(data.weightedAvgPrice);
                            decimal highPrice = TryParseDecimal(data.highPrice);
                            decimal lowPrice = TryParseDecimal(data.lowPrice);
                            decimal volume = TryParseDecimal(data.volume);

                            // Thêm thông tin vào dictionary
                            cryptoPrices[symbol] = new CryptoCurrency
                            {
                                Symbol = symbol,
                                Name = symbol, 
                                LastPrice = lastPrice,
                                PriceChange = priceChange,
                                PriceChangePercent = priceChangePercent,
                                WeightedAvgPrice = weightedAvgPrice,
                                HighPrice = highPrice,
                                LowPrice = lowPrice,
                                Volume = volume
                            };
                        }

                        // Lưu dictionary vào Redis dưới dạng JSON
                        var jsonPrices = JsonConvert.SerializeObject(cryptoPrices);
                        client.PushItemToList(redisListName, jsonPrices);

                        Console.WriteLine($"Đã thêm giá và thông tin vào Redis lúc {DateTime.Now}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi: {ex.Message}");
                    }

                    // Đợi 10 giây
                    Thread.Sleep(10000);
                }
            }
        }

        private static decimal TryParseDecimal(dynamic value)
        {
            decimal result = 0;
            if (value != null && decimal.TryParse(value.ToString(), out result))
            {
                return result;
            }
            return 0; // Trả về giá trị mặc định (0) nếu giá trị không hợp lệ hoặc null
        }

    }
}
