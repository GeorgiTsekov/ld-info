export class User {
    public firstName: string;
    public lastName: string;
    public email: string;
    public workedHours: number;

    constructor(
        firstName: string, 
        lastName: string,
        email: string,
        workedHours: number) {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.workedHours = workedHours;
    }
}