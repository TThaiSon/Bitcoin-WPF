using BitcoinPriceViewer.Models;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace BitcoinPriceViewer
{
    public partial class MainWindow : Window
    {
        private readonly RedisClient _redisClient;
        private readonly string _redisListName = "crypto-prices";
        private readonly DispatcherTimer _timer;
        private DateTime _startTime;
        private decimal _startPrice;
        private decimal _minPrice = decimal.MaxValue;
        private DateTime _minTime;
        private decimal _maxPrice = decimal.MinValue;
        private DateTime _maxTime;

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> TimeLabels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        private string _selectedSymbol = "BTCUSDT"; // Ký hiệu mặc định

        private readonly DispatcherTimer _marketUpdateTimer;

        private readonly Dictionary<string, string> _customCryptoNames = new Dictionary<string, string>
        {
            { "BTCUSDT", "Bitcoin" },
            { "ETHUSDT", "Ethereum" },
            { "SOLUSDT", "Solana" },
            { "BNBUSDT", "Binance Coin" },
            { "DOGEUSDT", "Dogecoin" },
            { "XRPUSDT", "XRP" },
            { "ADAUSDT", "Cardano" },
            { "SHIBUSDT", "Shiba Inu" },
            { "TRXUSDT", "Tron" },
            { "AVAXUSDT", "Avalanche" },
            { "TONUSDT", "Toncoin" },
            { "WBTCUSDT", "Wrapped Bitcoin" },
            { "LINKUSDT", "Chainlink" },
            { "BCHUSDT", "Bitcoin Cash" },
            { "SUIUSDT", "Sui" },
            { "DOTUSDT", "Polkadot" },
            { "APTUSDT", "Aptos" },
            { "NEARUSDT", "NEAR Protocol" },
            { "LTCUSDT", "Litecoin" },
            { "WBETHUSDT", "Wrapped Beacon ETH" },
            { "PEPEUSDT", "Pepe" },
            { "UNIUSDT", "Uniswap" },
            { "DAIUSDT", "Dai" },
            { "ICPUSDT", "Internet Computer" },
            { "TAOUSDT", "Bittensor" },
            { "RENDERUSDT", "Render" },
            { "ETCUSDT", "Ethereum Classic" },
            { "FETUSDT", "Artificial Superintelligence Alliance" },
            { "POLUSDT", "Polygon Ecosystem Token" },
            { "STXUSDT", "Stacks" },
            { "BONKUSDT", "Bonk" },
            { "AAVEUSDT", "Aave" },
            { "INJUSDT", "Injective" },
            { "FILUSDT", "Filecoin" },
            { "ARBUSDT", "Arbitrum" },
            { "FDUSDUSDT", "First Digital USD" },
            { "HBARUSDT", "Hedera Hashgraph" },
            { "FLOKIUSDT", "FLOKI" },
            { "TIAUSDT", "Celestia" },
            { "IMXUSDT", "ImmutableX" },
            { "VETUSDT", "VeChain" },
            { "FTMUSDT", "Fantom" },
            { "OPUSDT", "Optimism" },
            { "ATOMUSDT", "Cosmos" },
            { "RUNEUSDT", "THORChain" },
            { "SEIUSDT", "Sei" },
            { "GRTUSDT", "The Graph" },
            { "ENAUSDT", "Ethena" },
            { "PNUTUSDT", "Peanut the Squirel" },
            { "JUPUSDT", "Jupiter" },
            { "WLDUSDT", "Worldcoin" },
            { "PYTHUSDT", "Pyth Network" },
            { "THETAUSDT", "Theta Token" },
            { "MKRUSDT", "Maker" },
            { "RAYUSDT", "Raydium" },
            { "ALGOUSDT", "Algorand" }
        };

        private readonly Dictionary<string, string> _customToOriginalCryptoNames = new Dictionary<string, string>
        {
            { "Bitcoin", "BTCUSDT" },
            { "Ethereum", "ETHUSDT" },
            { "Solana", "SOLUSDT" },
            { "Binance Coin", "BNBUSDT" },
            { "Dogecoin", "DOGEUSDT" },
            { "XRP", "XRPUSDT" },
            { "Cardano", "ADAUSDT" },
            { "Shiba Inu", "SHIBUSDT" },
            { "Tron", "TRXUSDT" },
            { "Avalanche", "AVAXUSDT" },
            { "Toncoin", "TONUSDT" },
            { "Wrapped Bitcoin", "WBTCUSDT" },
            { "Chainlink", "LINKUSDT" },
            { "Bitcoin Cash", "BCHUSDT" },
            { "Sui", "SUIUSDT" },
            { "Polkadot", "DOTUSDT" },
            { "Aptos", "APTUSDT" },
            { "NEAR Protocol", "NEARUSDT" },
            { "Litecoin", "LTCUSDT" },
            { "Wrapped Beacon ETH", "WBETHUSDT" },
            { "Pepe", "PEPEUSDT" },
            { "Uniswap", "UNIUSDT" },
            { "Dai", "DAIUSDT" },
            { "Internet Computer", "ICPUSDT" },
            { "Bittensor", "TAOUSDT" },
            { "Render", "RENDERUSDT" },
            { "Ethereum Classic", "ETCUSDT" },
            { "Artificial Superintelligence Alliance", "FETUSDT" },
            { "Polygon Ecosystem Token", "POLUSDT" },
            { "Stacks", "STXUSDT" },
            { "Bonk", "BONKUSDT" },
            { "Aave", "AAVEUSDT" },
            { "Injective", "INJUSDT" },
            { "Filecoin", "FILUSDT" },
            { "Arbitrum", "ARBUSDT" },
            { "First Digital USD", "FDUSDUSDT" },
            { "Hedera Hashgraph", "HBARUSDT" },
            { "FLOKI", "FLOKIUSDT" },
            { "Celestia", "TIAUSDT" },
            { "ImmutableX", "IMXUSDT" },
            { "VeChain", "VETUSDT" },
            { "Fantom", "FTMUSDT" },
            { "Optimism", "OPUSDT" },
            { "Cosmos", "ATOMUSDT" },
            { "THORChain", "RUNEUSDT" },
            { "Sei", "SEIUSDT" },
            { "The Graph", "GRTUSDT" },
            { "Ethena", "ENAUSDT" },
            { "Peanut the Squirel", "PNUTUSDT" },
            { "Jupiter", "JUPUSDT" },
            { "Worldcoin", "WLDUSDT" },
            { "Pyth Network", "PYTHUSDT" },
            { "Theta Token", "THETAUSDT" },
            { "Maker", "MKRUSDT" },
            { "Raydium", "RAYUSDT" },
            { "Algorand", "ALGOUSDT" }
        };


        public MainWindow()
        {
            InitializeComponent();

            // Kết nối Redis
            _redisClient = new RedisClient("127.0.0.1", 6379);

            // Khởi tạo timer
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            // Khởi tạo biểu đồ
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Giá tiền điện tử",
                    Values = new ChartValues<decimal>()
                }
            };

            TimeLabels = new List<string>();
            YFormatter = value => value.ToString("C2");

            BitcoinChart.Series = SeriesCollection;
            DataContext = this;

            BitcoinChart.LegendLocation = LegendLocation.None;

            // Thiết lập ComboBox với các symbol tiền điện tử
            var symbols = new List<string> { "BTCUSDT", "ETHUSDT", "SOLUSDT", "BNBUSDT", "DOGEUSDT",
                            "XRPUSDT", "ADAUSDT", "SHIBUSDT", "TRXUSDT", "AVAXUSDT",
                            "TONUSDT", "WBTCUSDT", "LINKUSDT", "BCHUSDT", "SUIUSDT",
                            "DOTUSDT", "APTUSDT", "NEARUSDT", "LTCUSDT", "WBETHUSDT",
                            "PEPEUSDT", "UNIUSDT", "DAIUSDT", "ICPUSDT", "TAOUSDT",
                            "RENDERUSDT", "ETCUSDT", "FETUSDT", "POLUSDT", "STXUSDT",
                            "BONKUSDT", "AAVEUSDT", "INJUSDT", "ARBUSDT", "FILUSDT",
                            "FDUSDUSDT", "HBARUSDT", "TIAUSDT", "IMXUSDT", "FLOKIUSDT",
                            "VETCUSDT", "FTMUSDT", "OPUSDT", "ATOMUSDT", "RUNEUSDT",
                            "SEIUSDT", "GRTUSDT", "ENAUSDT", "PNUTUSDT", "JUPUSDT",
                            "WLDUSDT", "PYTHUSDT", "THETAUSDT", "MKRUSDT", "RAYUSDT",
                            "ALGOUSDT" };

            var customSymbols = symbols.Select(symbol => _customCryptoNames.ContainsKey(symbol) ? _customCryptoNames[symbol] : symbol).ToList();
            CryptoSymbolComboBox.ItemsSource = customSymbols;
            CryptoSymbolComboBox.SelectedItem = _customCryptoNames[_selectedSymbol];

            // Lấy giá trị khởi điểm
            _startTime = DateTime.Now;
            _startPrice = GetLatestPrice(_selectedSymbol);
            UpdatePriceInfo(_selectedSymbol);

            _marketUpdateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            _marketUpdateTimer.Tick += MarketUpdateTimer_Tick;
            _marketUpdateTimer.Start();

            // Lấy thông tin thị trường (market)
            UpdateMarketInfo();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdatePriceInfo(_selectedSymbol);
        }

        private decimal GetLatestPrice(string symbol)
        {
            // Lấy JSON của phần tử cuối cùng trong danh sách Redis
            string latestPriceJson = _redisClient.GetItemFromList(_redisListName, -1);

            if (string.IsNullOrEmpty(latestPriceJson))
            {
                return 0;
            }

            // Deserialize thành Dictionary với kiểu dữ liệu CryptoCurrency
            var prices = JsonConvert.DeserializeObject<Dictionary<string, CryptoCurrency>>(latestPriceJson);

            // Kiểm tra và trả về giá của symbol được chọn
            return prices != null && prices.TryGetValue(symbol, out CryptoCurrency cryptoCurrency) ? cryptoCurrency.LastPrice : 0;
        }

        private void UpdatePriceInfo(string symbol)
        {
            // Lấy dữ liệu từ Redis
            List<string> recentPricesJson = _redisClient.GetRangeFromList(_redisListName, -150, -1);

            List<decimal> recentPrices = new List<decimal>();

            // Lặp qua tất cả các bản ghi JSON từ Redis và lấy giá của symbol
            foreach (var json in recentPricesJson)
            {
                var prices = JsonConvert.DeserializeObject<Dictionary<string, CryptoCurrency>>(json);
                if (prices != null && prices.ContainsKey(symbol))
                {
                    var crypto = prices[symbol];
                    recentPrices.Add(crypto.LastPrice);
                }
            }

            // Cập nhật giá trị tối thiểu và tối đa từ các giá gần đây
            _minPrice = recentPrices.Min(); // Lấy giá trị min từ danh sách
            _maxPrice = recentPrices.Max(); // Lấy giá trị max từ danh sách
            _minTime = DateTime.Now; // Bạn có thể thay đổi thời gian nếu cần thiết
            _maxTime = DateTime.Now; // Bạn có thể thay đổi thời gian nếu cần thiết

            // Cập nhật UI
            Dispatcher.Invoke(() =>
            {
                // Cập nhật các TextBlock hiển thị thông tin giá trị
                CurrentDateTimeTextBlock.Text = $"{DateTime.Now:HH:mm:ss dd/MM/yyyy}";
                StartTimeTextBlock.Text = $"{_startTime:HH:mm:ss dd/MM/yyyy}";
                StartPriceTextBlock.Text = $"{_startPrice:F2}";
                MinTimeTextBlock.Text = $"{_minTime:HH:mm:ss dd/MM/yyyy}";
                MinPriceTextBlock.Text = $"{_minPrice:F2}";
                MaxTimeTextBlock.Text = $"{_maxTime:HH:mm:ss dd/MM/yyyy}";
                MaxPriceTextBlock.Text = $"{_maxPrice:F2}";

                SeriesCollection.Clear();

                // Vẽ biểu đồ với các giá trị gần đây
                for (int i = 1; i < recentPrices.Count; i++)
                {
                    var previousPrice = recentPrices[i - 1];
                    var currentPrice = recentPrices[i];

                    var lineSegment = new LineSeries
                    {
                        Values = new ChartValues<ObservablePoint>
                {
                    new ObservablePoint(i - 1, (double)previousPrice),
                    new ObservablePoint(i, (double)currentPrice)
                },
                        StrokeThickness = 2,
                        PointGeometry = DefaultGeometries.Square,
                        PointGeometrySize = 8,
                        Fill = System.Windows.Media.Brushes.Transparent,
                        Stroke = currentPrice > previousPrice ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red
                    };

                    SeriesCollection.Add(lineSegment);
                }

                BitcoinChart.Series = SeriesCollection;

                // Cập nhật nhãn thời gian cho biểu đồ
                TimeLabels.Clear();
                TimeLabels.AddRange(Enumerable.Range(1, recentPrices.Count).Select(i => i.ToString()));
                BitcoinChart.AxisX[0].Labels = TimeLabels;
            });
        }



        private void CryptoSymbolComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CryptoSymbolComboBox.SelectedItem is string selectedCustomName)
            {
                // Lấy tên gốc từ tên tùy chỉnh
                if (_customToOriginalCryptoNames.ContainsKey(selectedCustomName))
                {
                    _selectedSymbol = _customToOriginalCryptoNames[selectedCustomName];

                    // Lấy giá trị mới nhất từ Redis
                    _startPrice = GetLatestPrice(_selectedSymbol);

                    // Cập nhật các thông tin liên quan đến đồng tiền
                    UpdatePriceInfo(_selectedSymbol);
                }
            }
        }


        private void UpdateMarketInfo()
        {
            // Lấy các dữ liệu mới nhất từ Redis (giả sử lấy 25 mục cuối cùng)
            List<string> recentPricesJson = _redisClient.GetRangeFromList(_redisListName, -25, -1);

            // Tạo danh sách chứa thông tin tên đồng tiền và giá của nó
            var marketData = new List<CryptoCurrency>();

            // Tạo danh sách các symbol đã có trong marketData để tránh trùng lặp
            var existingSymbols = marketData.Select(x => x.Name).ToList();

            foreach (var json in recentPricesJson)
            {
                var prices = JsonConvert.DeserializeObject<Dictionary<string, CryptoCurrency>>(json);

                if (prices != null)
                {
                    foreach (var price in prices)
                    {
                        // Ánh xạ tên tùy chỉnh từ tên gốc
                        string customName = _customCryptoNames.ContainsKey(price.Key) ? _customCryptoNames[price.Key] : price.Key;

                        // Kiểm tra nếu symbol này đã có trong dữ liệu hiện tại (để tránh trùng lặp)
                        if (!existingSymbols.Contains(customName))
                        {
                            // Đường dẫn tới hình ảnh của đồng tiền điện tử
                            string imagePath = GetImagePathForCrypto(customName);

                            // Thêm dữ liệu vào danh sách thị trường
                            marketData.Add(new CryptoCurrency
                            {
                                Symbol = price.Key,
                                Name = customName,
                                LastPrice = price.Value.LastPrice,
                                PriceChange = price.Value.PriceChange,
                                PriceChangePercent = price.Value.PriceChangePercent,
                                WeightedAvgPrice = price.Value.WeightedAvgPrice,
                                HighPrice = price.Value.HighPrice,
                                LowPrice = price.Value.LowPrice,
                                Volume = price.Value.Volume,
                                ImagePath = imagePath
                            });

                            // Thêm symbol vào danh sách existingSymbols
                            existingSymbols.Add(customName);
                        }
                    }
                }
            }

            // Cập nhật UI với dữ liệu mới
            Dispatcher.Invoke(() =>
            {
                // Xóa dữ liệu cũ trong DataGrid trước khi cập nhật
                CryptoDataGrid.ItemsSource = null;

                // Cập nhật lại DataGrid với dữ liệu mới
                CryptoDataGrid.ItemsSource = marketData;
            });
        }


        private void MarketUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateMarketInfo();
        }

        private string GetImagePathForCrypto(string customName)
        {
            // Đường dẫn hình ảnh cho mỗi đồng tiền điện tử
            var imagePaths = new Dictionary<string, string>
            {
                { "Bitcoin", "https://bin.bnbstatic.com/static/assets/logos/BTC.png" },
                { "Ethereum", "https://bin.bnbstatic.com/static/assets/logos/ETH.png" },
                { "Solana", "https://bin.bnbstatic.com/static/assets/logos/SOL.png" },
                { "Binance Coin", "https://bin.bnbstatic.com/static/assets/logos/BNB.png" },
                { "Dogecoin", "https://bin.bnbstatic.com/static/assets/logos/DOGE.png" },
                { "XRP", "https://bin.bnbstatic.com/static/assets/logos/XRP.png" },
                { "Cardano", "https://bin.bnbstatic.com/static/assets/logos/ADA.png" },
                { "Tron", "https://bin.bnbstatic.com/static/assets/logos/TRX.png" },
                { "Shiba Inu", "https://bin.bnbstatic.com/static/assets/logos/SHIB.png" },
                { "Avalanche", "https://bin.bnbstatic.com/static/assets/logos/AVAX.png" },
                { "Toncoin", "https://bin.bnbstatic.com/static/assets/logos/BTC.png" },
                { "Wrapped Bitcoin", "https://bin.bnbstatic.com/static/assets/logos/TON.png" },
                { "Chainlink", "https://bin.bnbstatic.com/static/assets/logos/LINK.png" },
                { "Sui", "https://bin.bnbstatic.com/static/assets/logos/SUI.png" },
                { "Bitcoin Cash", "https://bin.bnbstatic.com/static/assets/logos/BCH.png" },
                { "Polkadot", "https://bin.bnbstatic.com/static/assets/logos/DOT.png" },
                { "Aptos", "https://bin.bnbstatic.com/static/assets/logos/APT.png" },
                { "NEAR Protocol", "https://bin.bnbstatic.com/static/assets/logos/NEAR.png" },
                { "Litecoin", "https://bin.bnbstatic.com/static/assets/logos/LTC.png" },
                { "Wrapped Beacon ETH", "https://bin.bnbstatic.com/static/assets/logos/WBETH.png" },
                { "Dai", "https://bin.bnbstatic.com/static/assets/logos/DAI.png" },
                { "Pepe", "https://bin.bnbstatic.com/static/assets/logos/PEPE.png" },
                { "Uniswap", "https://bin.bnbstatic.com/static/assets/logos/UNI.png" },
                { "Internet Computer", "https://bin.bnbstatic.com/static/assets/logos/ICP.png" },
                { "Bittensor", "https://bin.bnbstatic.com/static/assets/logos/TAO.png" },
                { "Render", "https://bin.bnbstatic.com/static/assets/logos/RENDER.png" },
                { "Ethereum Classic", "https://bin.bnbstatic.com/static/assets/logos/ETC.png" },
                { "Artificial Superintelligence Alliance", "https://bin.bnbstatic.com/static/assets/logos/FET.png" },
                { "Polygon Ecosystem Token", "https://bin.bnbstatic.com/static/assets/logos/POL.png" },
                { "Stacks", "https://bin.bnbstatic.com/static/assets/logos/STX.png" },
                { "Bonk", "https://bin.bnbstatic.com/static/assets/logos/BONK.png" },
                { "Aave", "https://bin.bnbstatic.com/static/assets/logos/AAVE.png" },
                { "Injective", "https://bin.bnbstatic.com/static/assets/logos/INJ.png" },
                { "Filecoin", "https://bin.bnbstatic.com/static/assets/logos/FIL.png" },
                { "Arbitrum", "https://bin.bnbstatic.com/static/assets/logos/ARB.png" },
                { "First Digital USD", "https://bin.bnbstatic.com/static/assets/logos/FDUSD.png" },
                { "Hedera Hashgraph", "https://bin.bnbstatic.com/static/assets/logos/HBAR.png" },
                { "FLOKI", "https://bin.bnbstatic.com/static/assets/logos/FLOKI.png" },
                { "Celestia", "https://bin.bnbstatic.com/static/assets/logos/TIA.png" },
                { "ImmutableX", "https://bin.bnbstatic.com/static/assets/logos/IMX.png" },
                { "VeChain", "https://bin.bnbstatic.com/static/assets/logos/VET.png" },
                { "Fantom", "https://bin.bnbstatic.com/static/assets/logos/FTM.png" },
                { "Optimism", "https://bin.bnbstatic.com/static/assets/logos/OP.png" },
                { "Cosmos", "https://bin.bnbstatic.com/static/assets/logos/ATOM.png" },
                { "THORChain", "https://bin.bnbstatic.com/static/assets/logos/RUNE.png" },
                { "Sei", "https://bin.bnbstatic.com/static/assets/logos/SEI.png" },
                { "The Graph", "https://bin.bnbstatic .com/static/assets/logos/GRT.png" },
                { "Ethena", "https://bin.bnbstatic.com/static/assets/logos/ENA.png" },
                { "Peanut the Squirel", "https://bin.bnbstatic.com/static/assets/logos/PNUT.png" },
                { "Jupiter", "https://bin.bnbstatic.com/static/assets/logos/JUP.png" },
                { "Worldcoin", "https://bin.bnbstatic.com/static/assets/logos/WLD.png" },
                { "Pyth Network", "https://bin.bnbstatic.com/static/assets/logos/PYTH.png" },
                { "Theta Token", "https://bin.bnbstatic.com/static/assets/logos/THETA.png" },
                { "Maker", "https://bin.bnbstatic.com/static/assets/logos/MKR.png" },
                { "Raydium", "https://bin.bnbstatic.com/static/assets/logos/RAY.png" },
                { "Algorand", "https://bin.bnbstatic.com/static/assets/logos/ALGO.png" }
            };

            // Kiểm tra nếu có hình ảnh cho đồng tiền này
            if (imagePaths.ContainsKey(customName))
            {
                return imagePaths[customName];
            }

            // Nếu không có, trả về hình ảnh mặc định
            return "https://bin.bnbstatic.com/static/assets/logos/BTC.png";
        }

        public class ValueToColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is double priceChangePercent)
                {
                    if (priceChangePercent < 0)
                        return new SolidColorBrush(Colors.Red);
                    else
                        return new SolidColorBrush(Color.FromArgb(255, 14, 203, 129)); // #0ecb81 xanh lá
                }
                return new SolidColorBrush(Colors.White); // Mặc định màu trắng
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
