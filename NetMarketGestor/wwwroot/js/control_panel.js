function doRequest(method, resource, headers, body) {
    return new Promise((resolve, reject) => {
        let xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                try {
                    resolve(JSON.parse(this.responseText));
                } catch (e) {
                    console.error(e);
                    return this.responseText;
                }
                return this.response;
            } else if (this.readyState == 4 && this.status != 500) {
                resolve(JSON.parse(this.responseText));
            } else if (this.readyState == 4) {
                reject();
            }
        };
        xhr.open(method, resource);
        for (const prop in headers) {
            if (Object.hasOwnProperty.call(headers, prop)) {
                xhr.setRequestHeader(prop, headers[prop]);
            }
        }
        xhr.send(body);
    }).then((res) => { return res; }, () => { return false; });
}

async function apiLogin() {
    await doRequest('POST', '/cuentas/registrar', { 'Content-Type': 'application/json' }, atob("eyAiZW1haWwiOiAidXN1YXJpb0BlamVtcGxvLmNvbSIsICJwYXNzd29yZCI6ICJjb250ckExMjMqIiB9"));
    let tokenReq = await doRequest('POST', '/cuentas/login', { 'Content-Type': 'application/json' }, atob("eyAiZW1haWwiOiAidXN1YXJpb0BlamVtcGxvLmNvbSIsICJwYXNzd29yZCI6ICJjb250ckExMjMqIiB9"));
    return tokenReq;
}

function buildOrderView(order, ordersContainer) {
    let orderConainer = document.createElement('div');
    orderConainer.className = 'order-conainer';
    orderConainer.innerHTML = `<div class="order-id">${orde.Id}</div><div class="order-address">${order.DireccionEntrega}</div><div class="order-status">${order.Estatus}</div><div class="order-user-email">${order.User.Email}</div>`;
    ordersContainer.append(orderConainer);
}

function buildOrdersView(orders) {
    let ordersContainer = document.getElementById('orders');
    ordersContainer.innerHTML = '';
    for (const order of orders) {
        buildOrderView(order, ordersContainer);
    }
}

async function loadOrders() {
    let userCredentials = await apiLogin();
    let orders = await doRequest('GET', 'api/pedidos', { 'Content-Type': 'application/json', 'Authorization': `Bearer ${userCredentials.token}` });
    console.log(orders);
    buildOrdersView(orders);
}

function documentReady() {
    if (document.readyState == 'complete') {
        console.log("Document is ready");
        setInterval(loadOrders, 10000);
    }
}

document.onreadystatechange = documentReady;