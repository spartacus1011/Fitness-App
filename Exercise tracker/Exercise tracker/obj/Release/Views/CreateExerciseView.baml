   M S B A M L   `   `   `  ���� X  TXceed.Wpf.Toolkit, Version=3.2.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit  K GExercise tracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Exercise_tracker.ViewsExercise_tracker.Views I)clr-namespace:Exercise_tracker.ViewModelsExercise_tracker.ViewModels /   )Exercise_tracker.Views.CreateExerciseView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     NwpfTool:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit     R NMvvmDialogs, Version=4.1.1.0, Culture=neutral, PublicKeyToken=3d3b9708589bc73e8md/https://github.com/fantasticfiasco/mvvm-dialogs  0local$clr-namespace:Exercise_tracker.Views  :
viewModels)clr-namespace:Exercise_tracker.ViewModels    � Title$  CreateExerciseView��$	��300��$	��300��$  MvvmDialogs.DialogServiceViews   IsRegistered . ���	#FF1E1E1E � 	Resources    	BoolToVis��%   &	        �� .���� .+��� $��Create a new exercise��$��Center=�
�������$��Calibri)�$��28(�$��10,10q�(   "Xceed.Wpf.Toolkit.WatermarkTextBox     	Watermark$ Exercise name��$
��10,5q�$��16(����� *ExerciseNameToAdd+� $
��10,5q�.+��� $��Exercise Type��
��������� $	��100��$��Right=�$��Center=� �� SelectedItem �� *SelectedExerciseType+���� *ExerciseTypeValues+� $
��10,5q�y��� *IsRepetitions+ �� 	Converter# [  .+��� $��Number of Reps��
�������%   Xceed.Wpf.Toolkit.IntegerUpDown  �   �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Maximum$	 500X�   Minimum$ 1X�$��50��$��Right=�   Value �� *Repetitions+� $
��10,5q�y��� *	IsTimed+# [  .+��� 	 �� Text$
	 Time��$
��Left=�$	��100�� �� *SelectedExerciseTimeUnits+���� *ExerciseTimeUnitValues+  $��50��$��Right=� �� *ExerciseTime+� $
��10,5q�y��� *IsSets+# [  .+��� $��Number of Sets��
�������  $��50��$��Right=� �� *	SetsCount+� $
��10,5q�y��� *IsSets+# [  .+��� $��Add Rest Time(Seconds)��
�������
 d� 	IsChecked
 �� *IsUsingRestTime+�� $	��100��$��Right=� �� *
RestTime+���� *ExerciseRestTimeValues+y��� *IsUsingRestTime+# [  � $
��10,5q�.+��� $��
Recurrence��
��������� $	��100��$��Right=� �� *SelectedExerciseRecurrence+���� *ExerciseRecurrenceValues+� $
��10,5q�y��� *
IsWeekly+# [  .+��� $	��100��$��Right=� �� *SelectedDayOfTheWeek+���� *DaysOfTheWeekValues+� $
��10,5q�y��� *	IsMonthly+# [  .+��� $	��100��$��Right=� �� *SelectedDayOfTheMonth+���� *DaysInThisMonth+� $
��10,5q�.+��� $!��Add to Roster straight away��$��Center=�
�������
 �� *IsUsedInRoster+� .+��� $��Create��$��60��$��25��$��Right=�$��25,15q����� *CloseDialogTrueCommand+�� $��Cancel�� �� IsCancel . $��60��$��25��$
��Left=�$��25,15q�