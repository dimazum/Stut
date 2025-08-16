import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { BackendService} from '../services/backend.service';
import { CalendarData, DayData } from '../models/models';

interface DayCell {
  date: Date | null;
  done: boolean;
  wordsRead: number;
  isToday: boolean;
}

@Component({
  selector: 'app-calendar-page',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './calendar-page.component.html',
  styleUrls: ['./calendar-page.component.css'],
})
export class CalendarPageComponent implements OnInit {
  private backendService = inject(BackendService);

  calendarGrid: DayCell[][] = [];
  today = new Date();
  currentMonthName = '';
  currentYear = 0;

  ngOnInit(): void {
    this.loadCalendar();
  }

loadCalendar(): void {
  this.backendService.getCalendar().subscribe({
    next: (calendarData) => this.buildCalendar(calendarData),
    error: (err) => {
      console.error('Ошибка при получении календаря:', err);
      // Строим пустой календарь на текущий месяц
      const today = new Date();
      this.buildCalendar({
        year: today.getFullYear(),
        month: today.getMonth(),
        days: [] // нет данных, все дни будут не выполнены
      });
    }
  });
}


buildCalendar(data: CalendarData): void {
  this.currentYear = data.year;
  this.currentMonthName = new Date(data.year, data.month).toLocaleString('default', { month: 'long' });

  const firstDayOfMonth = new Date(data.year, data.month, 1);
  const lastDayOfMonth = new Date(data.year, data.month + 1, 0);

  const calendar: DayCell[][] = [];
  let week: DayCell[] = [];

  // Смещение для первого дня недели (понедельник = 0)
  const jsDay = firstDayOfMonth.getDay();
  const startOffset = jsDay === 0 ? 6 : jsDay - 1; // переносим воскресенье в конец
  for (let i = 0; i < startOffset; i++) {
    week.push({ date: null, done: false, wordsRead: 0, isToday: false });
  }

  for (let day = 1; day <= lastDayOfMonth.getDate(); day++) {
    const dateObj = new Date(data.year, data.month, day);

    // ищем данные с бэка, если их нет — false и 0
    const dayData: DayData | undefined = data.days.find(d => d.date === dateObj.toISOString().split('T')[0]);

    week.push({
      date: dateObj,
      done: dayData?.done ?? false,
      wordsRead: dayData?.wordsRead ?? 0,
      isToday: this.isSameDay(dateObj, this.today)
    });

    if (week.length === 7) {
      calendar.push(week);
      week = [];
    }
  }

  // Дополняем последнюю неделю пустыми днями, если нужно
  while (week.length < 7 && week.length > 0) {
    week.push({ date: null, done: false, wordsRead: 0, isToday: false });
  }
  if (week.length) calendar.push(week);

  this.calendarGrid = calendar;
}


  isSameDay(d1: Date, d2: Date): boolean {
    return d1.getFullYear() === d2.getFullYear() &&
           d1.getMonth() === d2.getMonth() &&
           d1.getDate() === d2.getDate();
  }
}
