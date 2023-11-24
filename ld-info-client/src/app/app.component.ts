import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { TimeLog } from './timelogs/timelog.model';
import { User } from './users/user.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  loadedTimelogs: TimeLog[] = [];
  loadedTopUsers: User[] = [];
  isFetching = false;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.fetchTimelogs();
    this.fetchTopUsers();
  }

  onFetchTopUsers() {
    this.fetchTimelogs();
  }

  private fetchTopUsers() {
    this.isFetching = true;
    this.http
      .get<User[]>('https://localhost:7032/api/Users/GetTop10')
      .pipe(map(users => {
        return users.map(user => {
          return { ...user };
        })
      }))
      .subscribe(users => {
        this.isFetching = false;
        console.log(users)
        this.loadedTopUsers = users;
      });
  }

  onFetchTimelogs() {
    this.fetchTimelogs();
  }

  private fetchTimelogs() {
    this.isFetching = true;
    this.http
      .get<TimeLog[]>('https://localhost:7032/api/TimeLogs/GetAll')
      .pipe(map(timelogs => {
        return timelogs.map(timelog => {
          return { ...timelog };
        })
      }))
      .subscribe(timelogs => {
        console.log(timelogs)
        this.isFetching = false;
        this.loadedTimelogs = timelogs;
      });
  }
}
