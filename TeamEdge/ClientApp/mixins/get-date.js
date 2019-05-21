export default{
    methods:{
        getDate(date){
            var temp = new Date(date);
            return `${temp.getDate()}.${temp.getMonth() + 1}.${temp.getFullYear()}`
        },
        getDateTime(date){
            var temp = new Date(date);
            var hours = temp.getHours();
            hours = hours > 9 ? `${hours}` : `0${hours}`
            var minutes = temp.getMinutes();
            minutes = minutes > 9 ? `${minutes}` : `0${minutes}`
            var date = temp.getDate();
            date = date > 9 ? `${date}` : `0${date}`;
            var month = temp.getMonth() + 1;
            month = month > 9 ? `${month}` : `0${month}`;
            return `${hours}:${minutes} / ${date}.${month}.${temp.getFullYear()}`
        }
    }
}