import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { TimeLog } from "./timelog.model";
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class TimeLogsService {

    constructor(protected http: HttpClient) {

    }

    fetchTimelogs() {
        return this.http
            .get<TimeLog[]>('https://localhost:7032/api/TimeLogs/GetAll')
            .pipe(map(timelogs => {
                return timelogs.map(timelog => {
                    return { ...timelog };
                })
            }))
    }
}