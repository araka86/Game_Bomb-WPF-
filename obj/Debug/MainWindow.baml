   M S B A M L   `   `   `  ����D  @Bomb_Lab9, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Bomb_Lab9	Bomb_Lab9  5   	       BombGame.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Bomb_Lab9     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      (� $
��Left=�6    �� Left$ 14��6)   
 �� Top$ 18��6:   .��5
   
   � -   5         �Name$ 
LayoutRoot��5
       � ColumnDefinitions 5      �� 5      6%   �� 5      $	��280 �6#   61   .+�5      �� 
�����F�$��1q�6-   $��5q�6A   ._�5      � .+�5      �� -   5         ��Name$ canvasBackground��5      $	��490��6b   $��0,0,41,0q�6q   ��5      
� 5      .��5   "   �  � Color$ 	AliceBlue��6/    � Offset$	 0.5Q�6A   6P   � 5   "   $ White��6/   $	 0.7Q�6=   6L   5      .+�5      �� $	��395��6    $	 -74��6-   $	��530��6>   $ ��/city-2403422_960_720.webp��5      $ 4��6@   5      5      5      �� 5       $��1��6   
�����F�$��1q�6=   $��5q�6Q   ��5!      
� 5"       
� GradientOrigin$ 1,0.7&�6*   	 
� Center$	 1,0.7&�6A   
 
� RadiusX$
 1Q�6P    
� RadiusY$ 1Q�6\   .��5#      � $ Orange��6'   $ 0Q�67   6D   � 5$      $ White��6'   $ 1Q�65   6B   5%      ._�5'      �� $��15q�6   $��Center=�6)   $��Center=�6D   .+�5(      �� $��Impact)�6    $��26(�64   
����İ�russian Bomb6m   �� 5)      -   5         ��Name$ lblRate��5)       $��0,30,0,0q�61   $
��Wrap=�6C   $��Georgia)�6W   $��14(�6l   6|   �� 5*      -   5        $ lblSpeed��5*       $
��0,30q�62   $
��Wrap=�6@   $��Georgia)�6T   $��14(�6i   6y   �� 5+      -   5        $ 	lblStatus��5+       $
��Wrap=�63   $��Georgia)�6G   $��20(�6\   No bomb have dropped.6�   �� 5,      -   5         ��Name$ cmdStart��5,      $��5q�6/   $
��0,30q�6;   $��80��6I   $��
Start Game��6T   6�   �� 5-      -   5        $ 	cmdStart1��5-      $��5q�60   $��80��6<   $��
Reset Game��6G   6r   5/      51      52      53      55      