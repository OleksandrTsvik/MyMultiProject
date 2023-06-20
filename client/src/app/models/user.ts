export interface User {
    userName: string;
    email: string;
    registrationDate: Date;
    token: string;
}

export interface UserLogin {
    email: string;
    password: string;
}

export interface UserRegister {
    userName: string;
    email: string;
    password: string;
}