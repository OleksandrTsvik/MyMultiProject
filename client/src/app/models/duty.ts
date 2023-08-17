export interface Duty {
  id: string;
  position: number;
  title: string;
  description: string;
  isCompleted: boolean;
  dateCreation: Date;
  dateCompletion: Date | null;
  backgroundColor: string;
  fontColor: string;
}