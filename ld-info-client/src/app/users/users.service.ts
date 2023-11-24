import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from 'rxjs/operators';
import { User } from "./user.model";

@Injectable({ providedIn: 'root' })
export class UsersService {

    constructor(protected http: HttpClient) {

    }

    fetchUsers() {
        return this.http
            .get<User[]>('https://localhost:7032/api/Users/GetTop10')
            .pipe(map(users => {
                return users.map(user => {
                    return { ...user };
                })
            }))
    }
}