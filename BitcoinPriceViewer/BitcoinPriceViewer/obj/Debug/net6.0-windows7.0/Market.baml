   M S B A M L   `   `   `  ����M  IBitcoinPriceViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:BitcoinPriceViewerBitcoinPriceViewer  5   	       BitcoinPriceViewer.Market   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:BitcoinPriceViewer     5   	     � Title$  Market��5   	   $
��1920��6   $
��1080��6&    � 	Resources 5	       �� 
TargetType �� Property �� Value��%�   '��       �� 5
   
   " ��6   .�5      ��  � ��$ Center=�62   �� 5       � ��$ 5q�6'   �� 5       � ��
 �����6+   5      5      .��5      � ���#121212$
��auto��6    .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ CryptoDataGrid��5         AutoGenerateColumns$ False��5         HeadersVisibility$ Column=�5      ���#1C1C1E
�������   
IsReadOnly$
 True��5      	   RowBackground	 �#2A2A2C
   AlternatingRowBackground
 �#1C1C1E   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6L   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *Name+6L     5      $ Price��6%   $ * 6N    64   �� *Price+6N   5      5      5       