export class User {
    public email: string;
    public workedHours: number;

    constructor(
        email: string,
        workedHours: number) {
            this.email = email;
            this.workedHours = workedHours;
    }
}