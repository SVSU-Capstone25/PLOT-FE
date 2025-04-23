/**
 * @argument {object} fixture
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<boolean>}
 */
export async function updateFixtureInstance(fixture) {
  return fetch(
    `${window.location.protocol}//${window.location.hostname}:8085/api/fixtures/update-fixture-instance`,
    {
      method: "PATCH",
      headers: {
        Authorization: `Bearer ${await getCookie("Auth")}`,
        "Content-Type": "application/json; charset=utf-8",
      },
      credentials: "include",
      body: JSON.stringify(fixture),
    }
  ).then((data) => {
    if (!data.ok) throw new Error("Request responded with not OK!");
  });
}

/**
 * @argument {object} fixture
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<number>} The TUID of the created fixture instance
 */
export async function createFixtureInstance(fixture) {
  return fetch(
    `${window.location.protocol}//${window.location.hostname}:8085/api/fixtures/create-fixture-instance`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${await getCookie("Auth")}`,
        "Content-Type": "application/json; charset=utf-8",
      },
      credentials: "include",
      body: JSON.stringify(fixture),
    }
  ).then((data) => data.json());
}

/**
 * @argument {{FLOORSET_TUID: number, X1_POS: number, Y1_POS: number, X2_POS: number, Y2_POS: number}} employeeArea
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<boolean>}
 */
export async function bulkCreateEmployeeAreas(employeeArea) {
  return fetch(
    `${window.location.protocol}//${window.location.hostname}:8085/api/fixtures/bulk-add-employee-areas`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${await getCookie("Auth")}`,
        "Content-Type": "application/json; charset=utf-8",
      },
      credentials: "include",
      body: JSON.stringify(employeeArea),
    }
  ).then((data) => {
    if (!data.ok) throw new Error("Request responded with not OK!");
  });
}

/**
 * @argument {{FLOORSET_TUID: number, X1_POS: number, Y1_POS: number, X2_POS: number, Y2_POS: number}} employeeArea
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<boolean>}
 */
export async function bulkDeleteEmployeeAreas(employeeArea) {
  return fetch(
    `${window.location.protocol}//${window.location.hostname}:8085/api/fixtures/bulk-delete-employee-areas`,
    {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${await getCookie("Auth")}`,
        "Content-Type": "application/json; charset=utf-8",
      },
      credentials: "include",
      body: JSON.stringify(employeeArea),
    }
  ).then((data) => {
    if (!data.ok) throw new Error("Request responded with not OK!");
  });
}
