(() => {
    const d = {};
    const r = /cid=(\d+)/;
    function a(j) {
        const a = document.getElementById(j).getElementsByTagName("a");
        const l = [];
        for (let i = 0; i < a.length; i++) {
            l.push(Number(r.exec(a[i].href)[1]));
        }
        d[j] = l;
    }
    a("main");
    a("extra");
    a("side");
    console.log(d);
    return d;
})();

(() => {
    for (let i = 1; i <= 65; i++) {
        document.getElementById(`monm_${i}`).value = null;
        document.getElementById(`monum_${i}`).value = null;
        document.getElementById(`trnm_${i}`).value = null;
        document.getElementById(`trnum_${i}`).value = null;
        document.getElementById(`spnm_${i}`).value = null;
        document.getElementById(`spnum_${i}`).value = null;
    }
    for (let i = 1; i <= 20; i++) {
        document.getElementById(`exnm_${i}`).value = null;
        document.getElementById(`exnum_${i}`).value = null;
        document.getElementById(`sinm_${i}`).value = null;
        document.getElementById(`sinum_${i}`).value = null;
    }

    [{"n":"abc","c":5}].forEach(({ n, c }, i) => {
        document.getElemntById(`monm_${i + 1}`).value = n;
        document.getElementById(`monum_${i + 1}`).value = c;
    });

    const a = {"monsterCardId":[1,2,3,4,5]};
    for (let i = 1; i <= 65; i++) {
        document.querySelectorAll(`input#card_id_${i}`).forEach(e => {
            const am = a[e.name];
            e.value = am ? am[i - 1] : null;
        });
    }
})();