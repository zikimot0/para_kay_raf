   M S B A M L   `   `   `  ����  ReminderApp    ReminderApp.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
MainWindow��5   	   $	��500��6   $	��600��6)   $	��300��5   	   $	��400��6   .��5      �  � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��Welcome to ReminderApp��6   $��24(�62   $��Center=�6@   $��	0,20,0,20q�6]   $��0��6p   $��2��6}   $	��Top=�6�   �� 5   
   -   5         ��Name$ ReminderListBox��5      $��1��6+   $��0��68   $��20q�6H   $��Stretch=�6T   $��Stretch=�6r    �� SelectionMode$ Single=�6�   ��5      �� 5      .R�5      ��  �� Orientation$ Vertical=�6!   .+�5      �� $
��Bold"�6>   ��6$   �� *
DateTime+6>   �� 5       $��Italic$�6=   ��6$   �� *	Subject+6=   �� 5!      $
��Wrap=�6A   ��6$   �� *Description+6A   5"      5#      5%      �� 5(   
   $��1��6   $��1��6"   $��Right=�62   $��Center=�6N   $��10q�6i   .+�5*      �� -   5         ��Name$ ViewLoginLogsButton��5*   *   $��
Login Logs��6   $	��100��6G   $��30��6S   $��5q�6_   $y�	Collapsed=�6j   6�   5+      �� 5.   
   $ 
Horizontal=�6   $��2��6.   $��2��6;   $��Center=�6O   $��Bottom=�6l   $��20q�6�   .+�5/      �� -   5        $��Add Reminder��5/      $	��150��6,   $��40��68   $��10q�6D   6P   �� 50      -   5        $��Delete Reminder��50      $	��150��6/   $��40��6;   $��10q�6G   6S   �� 51      -   5        $��Logout��51      $	��150��6&   $��40��62   $��10q�6>   6J   52      53      54      