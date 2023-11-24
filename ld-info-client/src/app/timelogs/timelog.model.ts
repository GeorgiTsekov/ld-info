export class TimeLog {
    public userFirstName: string;
    public userLastName: string;
    public userEmail: string;
    public date: Date;
    public projectName: string;
    public hours: number;

    constructor(
        userFirstName: string, 
        userLastName: string,
        userEmail: string,
        date: Date,
        projectName: string,
        hours: number) {
            this.userFirstName = userFirstName;
            this.userLastName = userLastName;
            this.userEmail = userEmail;
            this.date = date;
            this.projectName = projectName;
            this.hours = hours;
    }
}