import Fixture from "./Fixture.js";

/**
 * @argument {Fixture} fixture
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<boolean>}
 */
export async function updateFixtureInstance(fixture) {
    console.log(JSON.stringify({
        updateFixture: fixture.toObject()
    }))
    return fetch(`http://${window.location.hostname}:8085/api/fixtures/update-fixture-instance`, {
        method: 'PATCH',
        headers: {
            'Authorization': `Bearer ${await getCookie('Auth')}`,
            'Content-Type': 'application/json; charset=utf-8',
            'Access-Control-Allow-Origin':'*'
        },
        // mode: 'no-cors',
        credentials: 'include',
        body: JSON.stringify(fixture.toObject())
    }).then((data) => data.ok);
}

/**
 * @argument {Fixture} fixture
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<boolean>}
 */
export async function createFixtureInstance(fixture) {
    return fetch(`http://${window.location.hostname}:8085/api/fixtures/create-fixture-instance`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${await getCookie('Auth')}`,
            'Content-Type': 'application/json; charset=utf-8',
            'Access-Control-Allow-Origin':'*'
        },
        // mode: 'no-cors',
        credentials: 'include',
        body: JSON.stringify(fixture.toObject())
    }).then((data) => data.ok);
}