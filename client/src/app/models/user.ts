export interface User {
    userName: string;
    email: string;
    registrationDate: Date;
    image?: string;
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