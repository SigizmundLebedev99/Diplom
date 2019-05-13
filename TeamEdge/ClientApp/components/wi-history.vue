<template>
    <div>
        <div v-for="(h,i) in history" :key="i">
            <span>{{h.dateOfCreation}}</span>
        </div>
    </div>
</template>

<script>
import {mapGetters} from 'vuex'
export default {
    data:()=>({
        history:[],
        loading:false
    }),
    methods:{
        fetchHistory(){
            this.loading = true;
            var string = `/api/history/project/${this.$route.parems.projId}`+
            `/code/${this.currentWI.code}/number/${this.currentWI.number}`
            this.$http.get(string).then(
                r=>{this.history = r.data; this.loading = false},
                r=>{console.log(r.response.data);this.loading = false;}
            )
        }
    },
    computed:{
        ...mapGetters({
            currentWI:'project/currentWI'
        })
    },
    watch:{
        currentWI(){
            this.fetchHistory();
        }
    }
}
</script>
