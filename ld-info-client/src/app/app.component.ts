import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { TimeLog } from './timelogs/timelog.model';
import { User } from './users/user.model';
import { TimeLogsService } from './timelogs/timelogs.service';
import { UsersService } from './users/users.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  loadedTimelogs: TimeLog[] = [];
  loadedTopUsers: User[] = [];
  isFetching = false;

  constructor(private http: HttpClient, private timelogsService: TimeLogsService, private usersService: UsersService) { }

  ngOnInit() {
    this.timelogsService.fetchTimelogs()
      .subscribe(timelogs => {
        this.loadedTimelogs = timelogs;
      });

    this.usersService.fetchUsers()
      .subscribe(users => {
        this.loadedTopUsers = users;
      });
  }

  onFetchTopUsers() {
    this.fetchTopUsers();
  }

  private fetchTopUsers() {
    this.usersService.fetchUsers()
      .subscribe(users => {
        this.loadedTopUsers = users;
      });
  }

  onFetchTimelogs() {
    this.timelogsService.fetchTimelogs()
      .subscribe(timelogs => {
        this.loadedTimelogs = timelogs;
      });
  }
}
