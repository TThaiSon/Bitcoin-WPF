   M S B A M L   `   `   `  ����M  IBitcoinPriceViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:BitcoinPriceViewerBitcoinPriceViewer  5   	   U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5   	   #    BitcoinPriceViewer.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:BitcoinPriceViewer     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	     � Title$  Bitcoin Price Viewer��5	   	   $
��auto��6&   $
��2600��64   ���#121212 � 	Resources 5       �� 
TargetType �� Property �� Value   TitleTextBlock  ValueTextBlock  ValueBorder �� Triggers P� Property P� Value��%M  '��       &	  �     &	 y    &	 �    '��     �� 5   
   " ��6   .�5      ��  � ��$ Center=�62   �� 5       � ��$ 5q�6'   �� 5       � ��
 �����6+   5      �� 5   
   " ��6'   .�5      ��  � ��$ 28(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � �� �#FFD7006+   �� 5       � ��$ Center=�64   �� 5       � ��$ 10q�6'   5      �� 5   
   " ��6'   .�5      ��  � ��$ SemiBold"�6+   �� 5       � �� �#FFFFFF6+   5      �� 5   
   " ��6$   .�5       ��  �@��CornerRadius$ 8��6-   �� 5!       � �� �#33353B6,   �� 5"       � ��$ 1q�60   �� 5#       �@��Padding$
 10,5q�6(   �� 5$       � ��$ 5q�6'   �� 5%       � �� �#1C1C1E6+   5&      �� 5(   
   " ��6   .�5)      ��  � �� �#181a206+   �� 5+       � ��
 �����6+    5-      P� 5/       �@��
IsSelected$
 True��60   . �50      ��  � �� �#181a2063   �� 52       � ��
 �����63   54      56      57      .��59      � ���#181a20 � RowDefinitions 5;   
   �� 5<      $
��Auto �6   �� 5>      $
��Auto �6   �� 5@      $��* �6   �� 5B      $
��Auto �6   .+�5G   
   � $��0��6   ���#181a20	 � ColumnDefinitions	 5H      �� 5I      $
��Auto �6#   �� 5K      $��* �6#   .+�5P      �� $,��&C:\Users\Son\Downloads\binancelogo.png��6   $��30��6D   $
��Left=�6R   $��10,0,0,0q�6m   �� 5S      $��BINANCE��6   #��[  6'   $
��Left=�6O   $��5,9,0,10q�6j   $��1��6|   5T      �� 5W   
   $��1��6   ���#181a20
�������.@�5X      �� $��Bitcoin Price��6   
����������#181a20$��22��6U   $��	-2,0,-2,0q�6a   $��Bottom=�6t   .��5Y      � ���#181a20$	��900��6,    5Z      �� 5[      $
��Auto �6(   �� 5\      $
��Auto �6(   �� 5]      $��* �6(   �� 5_      $
��Auto �6(   .+�5c      � $��1��6    
 � Orientation$
 
Horizontal=�6-   $��Center=�6F   $��10q�6c   .+�5d      �� $��Choose cryptocurrency: ��6$   #��[  6C   $��16(�6k   �� 5e      -   5         ��Name$ CryptoSymbolComboBox��5e   #   $	��120��5f   #   $��0,0,0,0q�5g   #   $��Center=�5h   #   5i   #   �� 5k      $��Current time: ��6$   �� 5l      #��[ 6!   ._�5m      �� -   5         ��Name$ CurrentDateTimeTextBlock��5m   (   #��[ 5o   (   ��5n   (   �� *CurrentDateTime+ �� StringFormat$ dd/MM/yyyy HH:mm:ss��5o   (   5p      �� 5r      $��Start time: ��6$   $��20,5,5,5q�68   �� 5s      #��[ 6!   ._�5t      �� -   5        $ StartTimeTextBlock��5t   (   #��[ 6D   5u      �� 5w      $��Starting price: ��6$   $��20,5,5,5q�6<   �� 5x      #��[ 6!   ._�5y      �� -   5        $ StartPriceTextBlock��5y   (   #��[ 6E   5z      5{      �� 5~      $��2��6   $��10q�6*    �� CornerRadius$ 10��66   ���#181a20���#FFFFFF$��1q�6s   ._�5      #  LiveCharts.Wpf.CartesianChart  -   5          Name$ BitcoinChart��5   -   &   LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$ Right=�6C   ���#181a20   AxisX 5�   "     LiveCharts.Wpf.Axis     Title$
 Time��6+      
Foreground �#FFFFFF   Labels 6M   �� *
TimeLabels+   	Separator 5�   &     LiveCharts.Wpf.Separator  5�   *      Stroke �#5e5e5e   StrokeThickness$	 0.2Q�6I   5�   #      AxisY 5�   "     $ Price��6+    �#FFFFFF   LabelFormatter 6N   �� *
YFormatter+ 5�   &     5�   *    �#5e5e5e$	 0.2Q�6I   5�   #   5�      5�      � 5�      $��3��6   $��10q�6(   	 5�      �� 5�      $��* �6/   �� 5�      $��* �6/   .+�5�      �� $��0��6%    �� Orientation$ 
Horizontal=�65   $
��Left=�6N   .+�5�      �� $��Lowest time: ��6(   �� 5�      #��[ 6%   ._�5�   "   �� -   5        $ MinTimeTextBlock��5�   ,   #��[ 6F   5�      �� 5�      $��Giá thấp nhất: ��6(   $��20,5,5,5q�6?   �� 5�      #��[ 6%   ._�5�   "   �� -   5        $ MinPriceTextBlock��5�   ,   #��[ 6G   
���  ��6o   5�      5�      �� 5�      $��1��6%   $ 
Horizontal=�65   $��Right=�6N   .+�5�      �� $��Highest time: ��6(   �� 5�      #��[ 6%   ._�5�   "   �� -   5        $ MaxTimeTextBlock��5�   ,   #��[ 6F   5�      �� 5�      $��Highest price: ��6(   $��20,5,5,5q�6?   �� 5�      #��[ 6%   ._�5�   "   �� -	   5        $ MaxPriceTextBlock��5�   ,   #��[ 6G   
��� � �6o   5�      5�      5�      5�      5�      �� 5�      $��Market��6   
����������#181a20$
��auto��6N   .��5�      � ���#181a20$
��1292��6,   .+�5�      &   System.Windows.Controls.DataGrid  -
   5          Name$ CryptoDataGrid��5�         AutoGenerateColumns$ False��5�         HeadersVisibility$ Column=�5�      ���#181a20   
IsReadOnly$
 True��5�         RowBackground �#181a20    AlternatingRowBackground  �#181a20
������ $��0q�5�      $��10q�5�      $!   HorizontalScrollBarVisibility$
! Auto=�5�      ""   VerticalScrollBarVisibility$
" Auto=�5�      #   Columns# 5�      4  .System.Windows.Controls.DataGridTemplateColumn  5�      ,  &System.Windows.Controls.DataGridColumn$   Header$$ Image��65   %   Width5  /System.Windows.Controls.DataGridLengthConverter$% 50 6D   &   CellTemplate& 5�   "   �� 5�   &   .R�5�   *   �� $��30��6M   $��30��6X   ��60   �� *	ImagePath+6X   5�   '   5�      0	  *System.Windows.Controls.DataGridTextColumn	  5�      $$ Symbol��61   $% 80 6\   ' 	  
Foreground
' �����1
  +System.Windows.Controls.DataGridBoundColumn( 
  Binding( 6A   �� *Symbol+6g   	  5�      $
$ Name��61   $	% 340 6X   ) 	  
FontWeight$
) Bold"�6d   * 	  FontSize$* 16Q�6v   
' �����( 6?   �� *Name+6�   	  5�      $$ Price��61   $	% 200 6v   $) DemiBold"�6�   $* 16Q�6�   
' �����( 6@   �� *	LastPrice+$ $ #0.00��6�   	  5�      $$ Price Change (%)��61   $	% 200 6�   $* 16Q�6�   
' �����( 6K   �� *PriceChange+$ $#0.00��6�   	  5�      $$ Price Change Percent 24h��61   $	% 200 6�   $* 16Q�6�   $) DemiBold"�6�   ( 6S   �� *PriceChangePercent+$ #0.00 %��+   	CellStyle+ 5�   "   �� 5�   &   *  $System.Windows.Controls.DataGridCell"  6,    5�   *   �� 5�   .   , �� Binding, 6:   �� *PriceChangePercent+- �� Value- 06a   .a�5�   2   ��  � ��
 �  ��6O   5�   /   �� 5�   .   , 6:   �� *PriceChangePercent+- 06a   .a�5�   2   ��  � ��
 �  ��6O   5�   /   5�   '   5�      	  5�      $$ Average Price��61   $	% 200 6�   $* 16Q�6�   
' �����( 6H   �� *WeightedAvgPrice+$ $ #0.00��6�   	  5�      $$ 
High Price��61   $	% 200 6{   $* 16Q�6�   
' �����( 6E   �� *	HighPrice+$ $ #0.00��6�   	  5�      $$ 	Low Price��61   $	% 200 6y   $* 16Q�6�   
' �����( 6D   �� *
LowPrice+$ $ #0.00��6�   	  5�      $$ Volume��61   $	% 200 6t   $* 16Q�6�   
' �����( 6A   �� *Volume+$ #0.00 B��6�   5�      5�      5�      5�      5�      5�      