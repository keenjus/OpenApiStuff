/**
 * This file was auto-generated by openapi-typescript.
 * Do not make direct changes to the file.
 */

export type paths = {
    "/Test": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get: {
            parameters: {
                query?: never;
                header?: never;
                path?: never;
                cookie?: never;
            };
            requestBody?: never;
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": components["schemas"]["Entity"][];
                        "application/json": components["schemas"]["Entity"][];
                        "text/json": components["schemas"]["Entity"][];
                    };
                };
            };
        };
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
};
export type webhooks = Record<string, never>;
export type components = {
    schemas: {
        Entity: components["schemas"]["EntityPerson"] | components["schemas"]["EntityCompany"];
        EntityCompany: {
            /** @enum {string} */
            $type?: "Company";
            name: string;
            values?: string[] | null;
        };
        EntityPerson: {
            /** @enum {string} */
            $type?: "Person";
            firstName: string;
            lastName: string;
            /** @enum {string} */
            gender: "Male" | "Female";
            values?: string[] | null;
        };
    };
    responses: never;
    parameters: never;
    requestBodies: never;
    headers: never;
    pathItems: never;
};
export type $defs = Record<string, never>;
export type operations = Record<string, never>;
