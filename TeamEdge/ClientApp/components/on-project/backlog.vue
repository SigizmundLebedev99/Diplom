<template>
    <v-layout column class="mt-2">
        <v-toolbar color="transparent" flat dense>
            <v-toolbar-title>Единицы работы</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon @click="preWICreating()">
                <v-icon>
                    add
                </v-icon>
            </v-btn>
            <v-btn icon>
                <v-icon>
                    search
                </v-icon>
            </v-btn>
        </v-toolbar>
        <v-container>
            <v-layout row wrap>
                <v-flex md6 xs12>
                    <v-list>
                        <v-list-tile v-for="(w,i) in items" :key="i" @click="$router.push({name:w.code, params:{number:w.number}})">
                            <v-list-tile-title>{{w.code}}-{{w.number}}</v-list-tile-title>
                        </v-list-tile>
                    </v-list>
                </v-flex>
            </v-layout>
        </v-container>
    </v-layout>
</template>

<script>
import {mapActions, mapGetters} from 'vuex'
export default {
    mounted() {
        this.fetchItems();
    },
    data:()=>({
        items:[]
    }),
    methods:{
        ...mapActions({preWICreating:'createWorkItem/preWICreating'}),
        fetchItems(){
            this.$http.get(`/api/workitems/project/${this.$route.params.projId}/items`)
            .then(r=>this.items = r.data)
        }
    },
    computed:{
        ...mapGetters({project:'project/project'})
    }
}
</script>

