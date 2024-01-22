// the differences between model & ui-models are:
// 1. model is used for the data that is used to map to entities in the database
// 2. ui-models are used for the data that is used to map to the UI elements
//array of strings (each strign is an err message)
export interface IErrors {
    [err: string]: string[];
}