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
  foo: CalendarEvent[];
  @Input()
  day: Date;
  constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient.get<CalendarEvent[]>(baseUrl + 'calendar')
    .subscribe(response => {
      this.foo = response;
    });
  }

  delete(calEvent: CalendarEvent) {
    console.log("delete clicked");
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