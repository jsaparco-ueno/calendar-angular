import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http' 
import { Input } from '@angular/core';
@Component({
  selector: 'create',
  templateUrl: './create.component.html'
})
export class CreateComponent {
  @Input()
  day: Date;
  baseUrl: string;
  title: string;
  notes: string;
  startTime: Date;
  endTime: Date;

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {

  }

  saveEvent() {
    var eventToSave:CalendarEvent = {
      id: '00000000-0000-0000-0000-000000000000',
      title: this.title,
      notes: this.notes,
      startTime: this.startTime,
      endTime: this.endTime,
      isDeleted: false
    };

    this.httpClient.post(this.baseUrl + 'calendar/create', eventToSave)
      .subscribe({
        next: response => {
          console.log('Event created.');
        },
        error: error => {
          console.error('Errors occurred while trying to create an event.')
        }
      });
  }
}

class CalendarEvent {
  id: string;
  title: string;
  notes: string;
  startTime: Date;
  endTime: Date;
  isDeleted: boolean;
}