<template>
    <div class="flex-row">
        <v-avatar class="mr-2 center" color="primary" size="30">
            <span v-if="!user.avatar" class="white--text text-xs-center" medium>
                {{`${user.name.split(' ').map(s=>s[0]).join('')}`}}
            </span>
            <v-img v-else :src="user.avatar"/>
        </v-avatar>
        <span class="center">{{user.name}}</span>
    </div>
</template>

<script>
export default {
props:[
    'user'
]
}
</script>

