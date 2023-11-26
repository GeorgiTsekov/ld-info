import { Component, OnDestroy, OnInit, ViewChild, ElementRef } from '@angular/core';
import { TimeLog } from './timelogs/timelog.model';
import { User } from './users/user.model';
import { TimeLogsService } from './timelogs/timelogs.service';
import { UsersService } from './users/users.service';
import { Subscription } from 'rxjs';
import { TimelogParams } from './timelogs/timelog.params';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})

export class AppComponent implements OnInit, OnDestroy {
  loadedTimelogs: TimeLog[] = [];
  loadedTopUsers: User[] = [];
  isTimelogsFetching = false;
  isUsersFetching = false;
  timeLogError = null;
  userError = null;
  private timelogErrorSub: Subscription;
  private userErrorSub: Subscription;
  @ViewChild('fromDateInput') fromDateInput: ElementRef;
  @ViewChild('toDateInput') toDateInput: ElementRef;
  @ViewChild('sortByInput') sortByInput: ElementRef;
  @ViewChild('isAscendingInput') isAscendingInput: ElementRef;
  @ViewChild('pageNumberInput') pageNumberInput: ElementRef;
  @ViewChild('pageSizeInput') pageSizeInput: ElementRef;

  constructor(private http: HttpClient, private timelogsService: TimeLogsService, private usersService: UsersService) { }

  ngOnInit() {
    this.timelogErrorSub = this.timelogsService.error.subscribe(errorMessage => {
      this.timeLogError = errorMessage;
    });

    this.userErrorSub = this.usersService.error.subscribe(errorMessage => {
      this.userError = errorMessage;
    });

    this.isTimelogsFetching = true;
    this.timelogsService.fetchTimelogs()
      .subscribe(
        timelogs => {
          this.isTimelogsFetching = false;
          this.loadedTimelogs = timelogs;
        },
        error => {
          this.isTimelogsFetching = false;
          this.timeLogError = error.message;
        });

    this.isUsersFetching = true;
    this.usersService.fetchUsers()
      .subscribe(
        users => {
          this.isUsersFetching = false;
          this.loadedTopUsers = users;
        },
        error => {
          this.isUsersFetching = false;
          this.userError = error.message;
        });
  }

  onRegenerateUsers() {
    this.usersService.resetUsers().subscribe(response => {
      console.log(response);

      this.resetDateInputs();
      
      this.ngOnInit();
    });
  }

  onFetchFilter(timelogParams?: TimelogParams) {
    this.usersService.fetchUsers(
      timelogParams?.fromDate,
      timelogParams?.toDate,
      timelogParams?.sortBy,
      timelogParams?.isAscending,
      timelogParams?.pageNumber,
      timelogParams?.pageSize)
      .subscribe(
        users => {
          console.log(users)
          this.loadedTopUsers = users;
        },
        error => {
          this.isUsersFetching = false;
          this.userErrorSub = error.message;
        });

    this.timelogsService.fetchTimelogs(
      timelogParams?.fromDate,
      timelogParams?.toDate,
      timelogParams?.sortBy,
      timelogParams?.isAscending,
      timelogParams?.pageNumber,
      timelogParams?.pageSize)
      .subscribe(
        timelogs => {
          console.log(timelogs)
          this.loadedTimelogs = timelogs;
        },
        error => {
          this.isTimelogsFetching = false;
          this.timeLogError = error.message;
        });
  }

  onFetchTopUsers() {
    this.usersService.fetchUsers()
      .subscribe(
        users => {
          console.log(users)
          this.loadedTopUsers = users;
        },
        error => {
          this.isUsersFetching = false;
          this.userErrorSub = error.message;
        });
  }

  onFetchTimelogs() {
    this.timelogsService.fetchTimelogs()
      .subscribe(
        timelogs => {
          console.log(timelogs)
          this.loadedTimelogs = timelogs;
        },
        error => {
          this.isTimelogsFetching = false;
          this.timeLogError = error.message;
        });
  }

  onHandleError() {
    this.timeLogError = null;
    this.userError = null;
  }

  ngOnDestroy() {
    this.timelogErrorSub.unsubscribe();
    this.userErrorSub.unsubscribe();
  }

  private resetDateInputs() {
    this.fromDateInput.nativeElement.value = null; 
    this.toDateInput.nativeElement.value = null; 
    this.isAscendingInput.nativeElement.value = null; 
    this.sortByInput.nativeElement.value = null; 
    this.pageNumberInput.nativeElement.value = null; 
    this.pageSizeInput.nativeElement.value = null; 
    
  }
}
