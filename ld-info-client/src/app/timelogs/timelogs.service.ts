import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { TimeLog } from "./timelog.model";
import { map, catchError } from 'rxjs/operators';
import { Subject, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TimeLogsService {
    error = new Subject<string>();

    constructor(protected http: HttpClient) {

    }

    fetchTimelogs(fromDate?: string, toDate?: string, sortBy?: string, isAscending?: boolean, pageNumber?: number, pageSize?: number) {
        let searchParams = new HttpParams();

        if (fromDate != '' && fromDate != undefined) {
            searchParams = searchParams.append('fromDate', fromDate);
        }
        if (toDate != '' && toDate != undefined) {
            searchParams = searchParams.append('toDate', toDate);
        }
        if (sortBy != '' && sortBy != undefined) {
            searchParams = searchParams.append('sortBy', sortBy);
        }
        if (!isAscending && isAscending != undefined) {
            searchParams = searchParams.append('isAscending', isAscending);
        }
        if (pageNumber != 0 && pageNumber != undefined) {
            searchParams = searchParams.append('pageNumber', pageNumber);
        }
        if (pageSize != 0 && pageSize != undefined) {
            searchParams = searchParams.append('pageSize', pageSize);
        }

        return this.http
            .get<TimeLog[]>(
                'https://localhost:7032/api/TimeLogs/GetAll',
                {
                    params: searchParams
                }
            )
            .pipe(map(timelogs => {
                return timelogs.map(timelog => {
                    return { ...timelog };
                })
            }),
                catchError(errorRes => {
                    // Send to analytics server
                    return throwError(errorRes);
                })
            );
    }
}