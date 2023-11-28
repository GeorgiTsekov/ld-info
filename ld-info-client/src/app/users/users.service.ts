import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { User } from "./user.model";
import { map, catchError} from 'rxjs/operators';
import { Subject, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UsersService {
    error = new Subject<string>();

    constructor(protected http: HttpClient) {

    }

    fetchUsers(fromDate?: string, toDate?: string, sortBy?: string, isAscending?: boolean, pageNumber?: number, pageSize?: number) {
        let searchParams = new HttpParams();

        if (fromDate != '' && fromDate != undefined) {
            searchParams = searchParams.append('fromDate', fromDate);
        }
        if (toDate != '' && toDate != undefined) {
            searchParams = searchParams.append('toDate', toDate);
        }

        return this.http
            .get<User[]>('https://localhost:7032/api/Users/GetTop10',
            {
                params: searchParams
            })
            .pipe(map(users => {
                return users.map(user => {
                    return { ...user };
                })
            }),
                catchError(errorRes => {
                    return throwError(errorRes);
                }));
    }

    resetUsers() {
        return this.http
            .get('https://localhost:7032/api/Seeder/GenerateNewData');
    }

    getUserByEmail(email: string, fromDate?: string, toDate?: string, sortBy?: string, isAscending?: boolean, pageNumber?: number, pageSize?: number) {
        let searchParams = new HttpParams().append('email', email);

        if (fromDate != '' && fromDate != undefined) {
            searchParams = searchParams.append('fromDate', fromDate);
        }
        if (toDate != '' && toDate != undefined) {
            searchParams = searchParams.append('toDate', toDate);
        }

        return this.http
            .get<User>(`https://localhost:7032/api/Users/GetByEmail`,
            {
                params: searchParams
            })
            .pipe(map(user => {
                return {...user}
            }),
            catchError(errorRes => {
                return throwError(errorRes);
            }));
    }
} 