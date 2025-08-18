import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { BackendService } from '../services/backend.service';
import { CalendarData, DayData } from '../models/models';
import { FormsModule } from '@angular/forms';

interface DayCell {
  lessonId: number
  date: Date | null;
  done: boolean;
  rewarded: boolean;
  wordsRead: number;
  isToday: boolean;
}

@Component({
  selector: 'app-calendar-page',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule],
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
          days: [],
        });
      },
    });
  }

  private formatDateLocal(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  buildCalendar(data: CalendarData): void {
    this.currentYear = data.year;
    this.currentMonthName = new Date(data.year, data.month).toLocaleString(
      'default',
      { month: 'long' }
    );

    const firstDayOfMonth = new Date(data.year, data.month, 1);
    const lastDayOfMonth = new Date(data.year, data.month + 1, 0);

    const calendar: DayCell[][] = [];
    let week: DayCell[] = [];

    // Смещение для первого дня недели (понедельник = 0)
    const jsDay = firstDayOfMonth.getDay();
    const startOffset = jsDay === 0 ? 6 : jsDay - 1; // воскресенье переносим в конец

    for (let i = 0; i < startOffset; i++) {
      week.push({lessonId: 0, date: null, done: false, rewarded: false, wordsRead: 0, isToday: false });
    }

    for (let day = 1; day <= lastDayOfMonth.getDate(); day++) {
      const dateObj = new Date(data.year, data.month, day);
      const dateStr = this.formatDateLocal(dateObj);

      // ищем данные с бэка
      const dayData: DayData | undefined = data.days.find(
        (d) => d.date === dateStr
      );

      week.push({
        lessonId: dayData?.lessonId ?? 0,
        date: dateObj,
        done: dayData?.done ?? false,
        rewarded: dayData?.rewarded ?? false,
        wordsRead: dayData?.wordsRead ?? 0,
        isToday: this.isSameDay(dateObj, this.today),
      });

      if (week.length === 7) {
        calendar.push(week);
        week = [];
      }
    }

    // Дополняем последнюю неделю пустыми днями
    while (week.length < 7 && week.length > 0) {
      week.push({lessonId: 0, date: null, done: false,rewarded: false, wordsRead: 0, isToday: false });
    }
    if (week.length) calendar.push(week);

    this.calendarGrid = calendar;
  }

  isSameDay(d1: Date, d2: Date): boolean {
    return (
      d1.getFullYear() === d2.getFullYear() &&
      d1.getMonth() === d2.getMonth() &&
      d1.getDate() === d2.getDate()
    );
  }

  onRewardChange(day: DayCell , event: Event) {
  const input = event.target as HTMLInputElement;
  const checked = input.checked;

  
  this.backendService.rewardLesson(day.lessonId, checked ).subscribe();
}

}
