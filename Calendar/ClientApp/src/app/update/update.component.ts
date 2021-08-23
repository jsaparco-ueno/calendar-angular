import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http' 
import { Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'update',
  templateUrl: './update.component.html'
})
export class UpdateComponent {
  @Input()
  day: Date;
  id: string;
  baseUrl: string;
  title: string;
  notes: string;
  startTime: Date;
  endTime: Date;

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string, private activatedRoute: ActivatedRoute) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      this.id = params['id'];
    })

    this.httpClient.get<CalendarEvent>(this.baseUrl + 'calendar/' + this.id)
    .subscribe({
      next: response => {
        this.title = response.title;
        this.notes = response.notes;
        this.startTime = response.startTime;
        this.endTime = response.endTime;
      }
    })
  }

  saveEvent() {
    var eventToSave:CalendarEvent = {
      id: this.id,
      title: this.title,
      notes: this.notes,
      startTime: this.startTime,
      endTime: this.endTime,
      isDeleted: false
    };

    this.httpClient.put(this.baseUrl + 'calendar/update', eventToSave)
      .subscribe({
        next: response => {
          console.log('Event updated.');
        },
        error: error => {
          console.error('Errors occurred while trying to update an event.')
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