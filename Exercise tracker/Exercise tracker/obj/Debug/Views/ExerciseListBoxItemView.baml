   M S B A M L   `   `   `  ����K  GExercise tracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Exercise_tracker.ViewsExercise_tracker.Views  5      I)clr-namespace:Exercise_tracker.ViewModelsExercise_tracker.ViewModels  5      4    .Exercise_tracker.Views.ExerciseListBoxItemView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      R NMvvmDialogs, Version=4.1.1.0, Culture=neutral, PublicKeyToken=3d3b9708589bc73e8md/https://github.com/fantasticfiasco/mvvm-dialogs  5      0local$clr-namespace:Exercise_tracker.Views  5      :
viewModels)clr-namespace:Exercise_tracker.ViewModels  5      $  MvvmDialogs.DialogServiceViews    IsRegistered  .  � 	Resources 5         	BoolToVis��%   &	        �� 5   
   6'   5      .��5      � $
��auto��6   $��0,0,0,0q�6   ��5   
   �� 5       �� 
TargetType" �6   .�5      ��  �� Property � �� �� Value �	#FF9191916C    �� Triggers 5      �� 5       �� Binding 6"   �� *OnTime+ �� Value true6>   .a�5      ��  � �� �	#FFF7C2346K   5      �� 5       6"   �� *Late+ true6<   .a�5      ��  � �� �	#FFFF40406K   5      �� 5       6"   �� *Early+ true6=   .a�5      ��  � �� �	#FF54B8666K   5!      5#      .+�5&   
   �� $
��Left=�6   $��Center=�6,   $��25,0,0,0q�6G   $��30��6Z   $��30��6f   ��6q   �� *MarkExerciseCompletedCommand+�� 5'   
   $
��Left=�6�   $��Center=�6�   $
��Wrap=�6�   $��70,0,0,0q�6�   
���������6   �� *RequiredSetsCount+ �� StringFormat$ {0} Sets Remaining��y�6Z   �� *IsSets+	 �� 	Converter#	 [  6�   �� 5(   
   $��Center=�62   $	��Top=�6O   $��0,10,0,0q�6g   $
��Wrap=�6y   $��36(�6�   
�������$��Calibri)�6�   ��6   �� *ExerciseName+6�   �� 5)   
   $��Center=�60   $	��Top=�6M   $��	0,58,0,10q�6e   $
��Wrap=�6x   $��22(�6�   
�������$��Calibri)�6�   ��6   �� *
ShownCount+6�   �� 5*   
   $��Right=�6   $��Center=�60   $��0,0,40,0q�6K   $
��Wrap=�6]   $��20(�6q   ��6   �� *DueTimeAsString+6�   o� 5+   
   $��20��6   $��20��6   $��Right=�6&   $	��Top=�6B   $	��5,5q�6Z   ���	#00000000$��12(�6~   .@�5,      m� $��:��6   $��20��6"   $��20��6-   $��Center=�69   $��Center=�6V   .@�5-      m� $
��Edit��6   
 m� Command
 6)   �� *EditThisExerciseCommand+m� 5.      $��Delete��6   
 6+   �� *DeleteThisExerciseCommand+5/      50      51      52      