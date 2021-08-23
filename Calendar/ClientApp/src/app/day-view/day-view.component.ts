import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http' 
import { Input } from '@angular/core';
@Component({
  selector: 'day-view',
  templateUrl: './day-view.component.html',
  styleUrls: ['./day-view.component.scss'],
})
export class DayViewComponent {
  title = 'Day View - Calendar App by JSU';
  calendarEvents: CalendarEvent[];
  @Input()
  day: Date;
  baseUrl: string;

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.get();
  }

  get() {
    this.httpClient.get<CalendarEvent[]>(this.baseUrl + 'calendar')
    .subscribe(response => {
      this.calendarEvents = response;
    });
  }

  delete(calEvent: CalendarEvent) {
    console.log('delete clicked');
    this.httpClient.delete(this.baseUrl + 'calendar/delete/' + calEvent.id)
    .subscribe({
      next: response => {
        console.log('Event deleted.');
      },
      error: error => {
        console.error('Errors occurred when deleting event.');
      }
    });
  }
}

interface CalendarEvent {
  id: string;
  title: string;
  notes: string;
  startTime: Date;
  endTime: Date;
  isDeleted: boolean;
}